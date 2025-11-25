namespace Evently.Application.Registrations.RegisterForEvent
{
    public sealed record RegisterForEventCommand(
        Guid EventId,
        string Email
    );
}
