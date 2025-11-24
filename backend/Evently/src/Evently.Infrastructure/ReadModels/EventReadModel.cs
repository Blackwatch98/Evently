namespace Evently.Infrastructure.ReadModels
{
    public class EventReadModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public DateTime ScheduledAt { get; set; }
    }
}
