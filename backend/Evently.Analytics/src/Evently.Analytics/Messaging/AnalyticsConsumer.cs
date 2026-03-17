using Evently.Analytics.Configs;
using Evently.Analytics.Infrastructure;
using Evently.Analytics.Infrastructure.Entities;
using Evently.Analytics.IntegrationEvents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Evently.Analytics.Messaging
{
    public sealed class AnalyticsConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<AnalyticsConsumer> _logger;
        private readonly RabbitMqConfiguration _config;
        private readonly ConnectionFactory _factory;

        private IConnection? _connection;
        private IChannel? _channel;

        public AnalyticsConsumer(
            IServiceScopeFactory scopeFactory,
            IOptions<RabbitMqConfiguration> config,
            ILogger<AnalyticsConsumer> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _config = config.Value;

            _factory = new ConnectionFactory
            {
                HostName = _config.HostName,
                Port = _config.Port,
                UserName = _config.UserName,
                Password = _config.Password
            };
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _connection = await _factory.CreateConnectionAsync(cancellationToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await _channel.QueueDeclareAsync(
                queue: _config.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: cancellationToken);

            _logger.LogInformation("Analytics consumer listening on queue '{Queue}'", _config.QueueName);

            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (_channel is null) throw new InvalidOperationException("Channel not initialized");

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (_, ea) =>
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AnalyticsDbContext>();

                try
                {
                    var messageType = GetHeader(ea, "message_type");
                    var json = Encoding.UTF8.GetString(ea.Body.ToArray());

                    switch (messageType)
                    {
                        case IntegrationEventNames.EventCreated:
                        {
                            var e = JsonSerializer.Deserialize<EventCreatedIntegrationEvent>(json)
                                                    ?? throw new InvalidOperationException("Invalid payload for event-created");

                            if (await AlreadyProcessed(db, e.Id, messageType, stoppingToken))
                            {
                                await _channel.BasicAckAsync(ea.DeliveryTag, false, ea.CancellationToken);
                                return;
                            }

                            await ApplyEventCreated(db, e, stoppingToken);
                            await MarkProcessed(db, e.Id, messageType, stoppingToken);
                            break;
                        }
                        case IntegrationEventNames.RegistrationCreated:
                        {
                            var e = JsonSerializer.Deserialize<RegistrationCreatedIntegrationEvent>(json)
                                ?? throw new InvalidOperationException("Invalid payload for registration-created");

                            if (await AlreadyProcessed(db, e.Id, messageType, stoppingToken))
                            {
                                await _channel.BasicAckAsync(ea.DeliveryTag, false, ea.CancellationToken);
                                return;
                            }

                            await ApplyRegistrationCreated(db, e, stoppingToken);
                            await MarkProcessed(db, e.Id, messageType, stoppingToken);
                            break;
                        }
                        default:
                            _logger.LogWarning("Unknown message_type: {MessageType}, Payload={Payload}", messageType, json);
                            break;
                    }
                    
                    await _channel.BasicAckAsync(ea.DeliveryTag, false, ea.CancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message");
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, requeue: true, ea.CancellationToken);
                }
            };

            await _channel.BasicConsumeAsync(
                queue: _config.QueueName,
                autoAck: false,
                consumer: consumer,
                cancellationToken: stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
                await Task.Delay(1000, stoppingToken);
        }

        private static string? GetHeader(BasicDeliverEventArgs ea, string key)
        {
            if (ea.BasicProperties.Headers is null) return null;
            if (!ea.BasicProperties.Headers.TryGetValue(key, out var value)) return null;

            return value switch
            {
                byte[] bytes => Encoding.UTF8.GetString(bytes),
                string s => s,
                _ => value?.ToString()
            };
        }

        private static DateTime Day(DateTime utc) =>
            new(utc.Year, utc.Month, utc.Day, 0, 0, 0, DateTimeKind.Utc);

        private static async Task<bool> AlreadyProcessed(AnalyticsDbContext db, Guid id, string type, CancellationToken ct)
            => await db.InboxMessages.AnyAsync(x => x.Id == id && x.Type == type && x.ProcessedOn != null, cancellationToken: ct);

        private static async Task MarkProcessed(AnalyticsDbContext db, Guid id, string type, CancellationToken ct)
        {
            var inbox = await db.InboxMessages.FindAsync([id], ct);
            if (inbox is null)
            {
                db.InboxMessages.Add(new InboxMessage
                {
                    Id = id,
                    Type = type,
                    ReceivedOn = DateTime.UtcNow,
                    ProcessedOn = DateTime.UtcNow
                });
            }
            else
            {
                inbox.ProcessedOn = DateTime.UtcNow;
            }

            await db.SaveChangesAsync(ct);
        }

        private static async Task ApplyEventCreated(AnalyticsDbContext db, EventCreatedIntegrationEvent e, CancellationToken ct)
        {
            var stat = await db.EventStats.FindAsync([e.EventId], ct);
            if (stat is null)
            {
                db.EventStats.Add(new EventStat
                {
                    EventId = e.EventId,
                    Title = e.Title,
                    ScheduledAt = e.ScheduledAt,
                    CreatedAt = e.OccurredOn,
                    RegistrationsCount = 0
                });
            }
            else
            {
                stat.Title = e.Title;
                stat.ScheduledAt = e.ScheduledAt;
            }

            var date = Day(e.OccurredOn);
            var daily = await db.DailyStats.FindAsync([date], ct);
            if (daily is null)
            {
                db.DailyStats.Add(new DailyStat { Date = date, EventsCreatedCount = 1, RegistrationsCreatedCount = 0 });
            }
            else daily.EventsCreatedCount += 1;

            await db.SaveChangesAsync(ct);
        }

        private static async Task ApplyRegistrationCreated(AnalyticsDbContext db, RegistrationCreatedIntegrationEvent e, CancellationToken ct)
        {
            var stat = await db.EventStats.FindAsync([e.EventId], ct);
            if (stat is null)
            {
                db.EventStats.Add(new EventStat
                {
                    EventId = e.EventId,
                    Title = e.Title,
                    ScheduledAt = DateTime.MinValue,
                    CreatedAt = e.OccurredOn,
                    RegistrationsCount = 1
                });
            }
            else stat.RegistrationsCount += 1;

            var date = Day(e.OccurredOn);
            var daily = await db.DailyStats.FindAsync([date], ct);
            if (daily is null)
            {
                db.DailyStats.Add(new DailyStat { Date = date, EventsCreatedCount = 0, RegistrationsCreatedCount = 1 });
            }
            else daily.RegistrationsCreatedCount += 1;

            await db.SaveChangesAsync(ct);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (_channel is not null) await _channel.CloseAsync(cancellationToken);
                if (_connection is not null) await _connection.CloseAsync(cancellationToken);
            }
            catch { /* ignore */ }

            await base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
            base.Dispose();
        }
    }
}
