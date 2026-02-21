namespace Evently.Infrastructure.Outbox
{
    public class OutboxMessage
    {
        public Guid IdOutboxMessage { get; set; }
        public DateTime OccurredOn { get; set; }
        public string Type { get; set; } = default!;
        public string Payload { get; set; } = default!;
        public DateTime? ProcessedOn { get; set; }
        public string? Error { get; set; }
    }
}
