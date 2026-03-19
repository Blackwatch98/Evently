using Evently.Application.Abstractions;
using Evently.Application.IntegrationEvents.Registrations;
using Evently.Domain.RegistrationAggregate.DomainEvents;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Evently.Application.IntegrationEvents;

namespace Evently.Infrastructure.Outbox
{
    public sealed class RegistrationCreatedToOutboxHandler : IDomainEventHandler<RegistrationCreated>
    {
        private readonly EventlyDbContext _dbContext;

        public RegistrationCreatedToOutboxHandler(EventlyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(RegistrationCreated domainEvent, CancellationToken cancellationToken = default)
        {
            var eventTitle = await _dbContext.Events
                .Where(e => e.IdEvent == domainEvent.EventId)
                .Select(e => e.Title)
                .FirstOrDefaultAsync(cancellationToken);

            var integrationEvent = new RegistrationCreatedIntegrationEvent(
                Id: Guid.NewGuid(),
                OccurredOn: DateTime.UtcNow,
                RegistrationId: domainEvent.RegistrationId,
                EventId: domainEvent.EventId,
                Title: string.IsNullOrEmpty(eventTitle) ? "Unknown" : eventTitle,
                RegisteredAt: domainEvent.OccuredAt
            );

            var outbox = new OutboxMessage
            {
                IdOutboxMessage = integrationEvent.Id,
                OccurredOn = integrationEvent.OccurredOn,
                Type = IntegrationEventNames.RegistrationCreated,
                Payload = JsonSerializer.Serialize(integrationEvent),
                ProcessedOn = null,
                Error = null
            };

            await _dbContext.OutboxMessages.AddAsync(outbox, cancellationToken);
        }
    }
}
