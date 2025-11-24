using Evently.Domain.EventAggregate;
using Microsoft.EntityFrameworkCore;

namespace Evently.Infrastructure
{
    public class EventlyDbContext : DbContext
    {
        public DbSet<Event> Events => Set<Event>();

        public EventlyDbContext(DbContextOptions<EventlyDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>(b =>
            {
                b.HasKey(e => e.Id);
                b.Property(e => e.Title).IsRequired().HasMaxLength(200);
                b.Property(e => e.Description).HasMaxLength(2000);
            });
        }
    }
}
