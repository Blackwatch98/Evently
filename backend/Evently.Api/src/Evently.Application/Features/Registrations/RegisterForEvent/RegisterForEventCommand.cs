namespace Evently.Application.Features.Registrations.RegisterForEvent;

public sealed record RegisterForEventCommand(
    Guid EventId,
    string Email
);
