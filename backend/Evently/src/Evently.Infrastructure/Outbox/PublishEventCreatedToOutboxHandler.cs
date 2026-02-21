using Evently.Application.Abstractions;
using Evently.Application.IntegrationEvents.Events;
using Evently.Domain.EventAggregate.DomainEvents;
using System.Text.Json;

namespace Evently.Infrastructure.Outbox
{
    public sealed class PublishEventCreatedToOutboxHandler : IDomainEventHandler<EventCreated>
    {
        private readonly EventlyDbContext _dbContext;

        public PublishEventCreatedToOutboxHandler(EventlyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(EventCreated domainEvent, CancellationToken cancellationToken = default)
        {
            var integrationEvent = new EventCreatedIntegrationEvent(
                Id: Guid.NewGuid(),
                OccurredOn: DateTime.UtcNow,
                EventId: domainEvent.EventId,
                Title: domainEvent.Title,
                ScheduledAt: domainEvent.ScheduledAt
            );

            var outboxMessage = new OutboxMessage
            {
                IdOutboxMessage = integrationEvent.Id,
                OccurredOn = integrationEvent.OccurredOn,
                Type = integrationEvent.GetType().FullName!,
                Payload = JsonSerializer.Serialize(integrationEvent)
            };

            await _dbContext.OutboxMessages.AddAsync(outboxMessage, cancellationToken);
        }
    }
}
