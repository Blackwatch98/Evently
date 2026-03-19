using Evently.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Evently.Infrastructure.Outbox;

public sealed class OutboxProcessor : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OutboxProcessor> _logger;

    public OutboxProcessor(IServiceProvider serviceProvider, ILogger<OutboxProcessor> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<EventlyDbContext>();
                var messageBus = scope.ServiceProvider.GetRequiredService<IMessageBus>();

                var messages = await dbContext.OutboxMessages
                    .Where(x => x.ProcessedOn == null)
                    .OrderBy(x => x.OccurredOn)
                    .Take(20)
                    .ToListAsync(stoppingToken);

                foreach (var msg in messages)
                {
                    try
                    {
                        await messageBus.PublishAsync(msg.Type, msg.Payload, stoppingToken);
                        msg.ProcessedOn = DateTime.UtcNow;
                    }
                    catch (Exception ex)
                    {
                        msg.Error = ex.Message;
                        _logger.LogError(ex, "Error while processing OutboxMessage {Id}", msg.IdOutboxMessage);
                    }
                }

                await dbContext.SaveChangesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Outbox processing loop failed");
            }

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}
