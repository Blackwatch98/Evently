namespace Evently.Application.IntegrationEvents.Registrations
{
    public sealed record RegistrationCreatedIntegrationEvent(
        Guid Id,
        DateTime OccurredOn,
        Guid RegistrationId,
        Guid EventId,
        DateTime RegisteredAt
    );
}
