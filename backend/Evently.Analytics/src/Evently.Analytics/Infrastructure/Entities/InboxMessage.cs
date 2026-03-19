namespace Evently.Analytics.Infrastructure.Entities
{
    public sealed class InboxMessage
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = default!;
        public DateTime ReceivedOn { get; set; }
        public DateTime? ProcessedOn { get; set; }
        public string? Error { get; set; }
    }
}
