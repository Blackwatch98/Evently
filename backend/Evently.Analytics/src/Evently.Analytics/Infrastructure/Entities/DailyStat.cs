namespace Evently.Analytics.Infrastructure.Entities
{
    public sealed class DailyStat
    {
        public DateTime Date { get; set; } // 00:00:00 UTC
        public int EventsCreatedCount { get; set; }
        public int RegistrationsCreatedCount { get; set; }
    }
}
