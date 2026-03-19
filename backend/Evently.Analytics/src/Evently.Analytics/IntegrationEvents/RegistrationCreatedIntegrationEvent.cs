namespace Evently.Analytics.IntegrationEvents
{
    public sealed record RegistrationCreatedIntegrationEvent(
        Guid Id,
        DateTime OccurredOn,
        Guid RegistrationId,
        Guid EventId,
        string Title,
        DateTime RegisteredAt
    );
}
