namespace Evently.Infrastructure.ReadModels
{
    public sealed class EventReadModel
    {
        public Guid IdReadModel { get; set; }
        public string Title { get; set; } = default!;
        public DateTime ScheduledAt { get; set; }
    }
}
