namespace Evently.Application.Features.Events.CreateEvent;

public sealed record CreateEventCommand(
    string Title,
    string Description,
    DateTime ScheduledAt,
    int Capacity
);
