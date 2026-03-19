using Evently.Analytics.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Evently.Analytics.Infrastructure
{
    public sealed class AnalyticsDbContext : DbContext
    {
        public AnalyticsDbContext(DbContextOptions<AnalyticsDbContext> options) : base(options) { }

        public DbSet<EventStat> EventStats => Set<EventStat>();
        public DbSet<DailyStat> DailyStats => Set<DailyStat>();
        public DbSet<InboxMessage> InboxMessages => Set<InboxMessage>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventStat>().HasKey(x => x.EventId);
            modelBuilder.Entity<DailyStat>().HasKey(x => x.Date);
            modelBuilder.Entity<InboxMessage>().HasKey(x => x.Id);

            modelBuilder.Entity<InboxMessage>()
                .HasIndex(x => new { x.Type, x.ProcessedOn });
        }
    }
}
