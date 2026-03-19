using Evently.Domain.Common;

namespace Evently.Domain.EventAggregate.DomainEvents;

public sealed record EventCreated(Guid EventId, string Title, DateTime ScheduledAt) : IDomainEvent
{
    public DateTime OccuredAt { get; set; } = DateTime.UtcNow;
}
