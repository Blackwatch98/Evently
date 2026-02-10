using Evently.Notifications.Configs;
using Evently.Notifications.IntegrationEvents;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Evently.Notifications
{
    public class EventCreatedConsumer : BackgroundService
    {
        private readonly ILogger<EventCreatedConsumer> _logger;
        private readonly RabbitMqConfiguration _config;
        private IChannel? _channel;
        
        private IConnection? _connection;
        private readonly ConnectionFactory _factory;

        public EventCreatedConsumer(
            IOptions<RabbitMqConfiguration> config,
            ILogger<EventCreatedConsumer> logger)
        {
            _config = config.Value;
            _logger = logger;

            _factory = new ConnectionFactory
            {
                HostName = _config.HostName,
                Port = _config.Port,
                UserName = _config.UserName,
                Password = _config.Password
            };
        }

        public async override Task StartAsync(CancellationToken cancellationToken)
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

            _logger.LogInformation(
                "Connected to RabbitMQ. Listening on queue '{QueueName}'",
                _config.QueueName);

            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (_channel is null)
            {
                throw new InvalidOperationException("Channel not initialized");
            }

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (sender, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var json = Encoding.UTF8.GetString(body);

                    var integrationEvent =
                        JsonSerializer.Deserialize<EventCreatedIntegrationEvent>(json);

                    if (integrationEvent is null)
                    {
                        _logger.LogWarning(
                            "Could not deserialize EventCreatedIntegrationEvent from message");
                    }
                    else
                    {
                        _logger.LogInformation(
                            "New event created: {Title} (Id: {EventId}) at {ScheduledAt}",
                            integrationEvent.Title,
                            integrationEvent.EventId,
                            integrationEvent.ScheduledAt);
                    }

                    await _channel.BasicAckAsync(
                        deliveryTag: ea.DeliveryTag,
                        multiple: false,
                        cancellationToken: ea.CancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while processing message");

                    await _channel.BasicNackAsync(
                        deliveryTag: ea.DeliveryTag,
                        multiple: false,
                        requeue: true,
                        cancellationToken: ea.CancellationToken);
                }
            };

            await _channel.BasicConsumeAsync(
                queue: _config.QueueName,
                autoAck: false,
                consumer: consumer,
                cancellationToken: stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (_channel is not null)
                    await _channel.CloseAsync(cancellationToken);

                if (_connection is not null)
                    await _connection.CloseAsync(cancellationToken);
            }
            catch
            {
                // ignorujemy b³êdy przy zamykaniu
            }

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