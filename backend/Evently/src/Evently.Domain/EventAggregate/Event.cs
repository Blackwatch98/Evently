using Evently.Domain.Abstractions;
using Evently.Domain.Events;

namespace Evently.Domain.EventAggregate
{
    public sealed class Event : Entity
    {
        private Event() { }
        public Guid IdEvent { get; private set; }
        public string Title { get; private set; } = default!;
        public string Description { get; private set; } = default!;
        public DateTime ScheduledAt { get; private set; }
        public int Capacity { get; private set; }

        private Event(string title, string description, DateTime scheduledAt, int capacity)
        {
            IdEvent = Guid.NewGuid();
            Title = title;
            Description = description;
            ScheduledAt = scheduledAt;
            Capacity = capacity;

            AddDomainEvent(new EventCreated(IdEvent, Title, ScheduledAt));
        }

        public static Event Create(string title, string description, DateTime scheduledAt, int capacity)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title cannot be empty.", nameof(title));
            }

            if (capacity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than zero.");
            }

            return new Event(title, description, scheduledAt, capacity);
        }

        public void UpdateDetails(string title, string description, DateTime scheduledAt, int capacity)
        {
            Title = title;
            Description = description;
            ScheduledAt = scheduledAt;
            Capacity = capacity;

            // Tu mógłby być np. EventUpdated
            // AddDomainEvent(new EventUpdated(Id, Title, ScheduledAt));
        }
    }
}
