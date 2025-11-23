using Evently.Domain.Abstractions;

namespace Evently.Domain.Events
{
    public sealed record EventCreated(Guid EventId, string Title, DateTime ScheduledAt) : IDomainEvent
    {
        public DateTime OccuredAt { get; set; } = DateTime.UtcNow;
    }
}
