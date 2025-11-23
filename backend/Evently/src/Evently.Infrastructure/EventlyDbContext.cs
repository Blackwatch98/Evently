using Evently.Domain.EventAggregate;
using Microsoft.EntityFrameworkCore;

namespace Evently.Infrastructure
{
    public class EventlyDbContext : DbContext
    {
        public EventlyDbContext(DbContextOptions<EventlyDbContext> options) : base(options)
        {
        }

        public DbSet<Event> Events => Set<Event>();

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
