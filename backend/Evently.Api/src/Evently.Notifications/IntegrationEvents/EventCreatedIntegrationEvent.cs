namespace Evently.Notifications.IntegrationEvents
{
    public sealed record EventCreatedIntegrationEvent(
        Guid Id,
        DateTime OccurredOn,
        Guid EventId,
        string Title,
        DateTime ScheduledAt
    );
}
