namespace Evently.Application.IntegrationEvents.Events;

public sealed record EventCreatedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid EventId,
    string Title,
    DateTime ScheduledAt
) : IntegrationEvent(Id, OccurredOn);
