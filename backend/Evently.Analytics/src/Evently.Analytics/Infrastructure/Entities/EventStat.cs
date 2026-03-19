namespace Evently.Analytics.Infrastructure.Entities
{
    public sealed class EventStat
    {
        public Guid EventId { get; set; }
        public string Title { get; set; } = default!;
        public DateTime ScheduledAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public int RegistrationsCount { get; set; }
    }
}
