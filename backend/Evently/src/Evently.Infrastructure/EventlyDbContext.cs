using Evently.Domain.EventAggregate;
using Evently.Infrastructure.ReadModels;
using Microsoft.EntityFrameworkCore;

namespace Evently.Infrastructure
{
    public class EventlyDbContext : DbContext
    {
        public DbSet<Event> Events => Set<Event>();
        public DbSet<EventReadModel> EventReadModels => Set<EventReadModel>();

        public EventlyDbContext(DbContextOptions<EventlyDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Event>(b =>
            {
                b.HasKey(e => e.Id);
                b.Property(e => e.Title).IsRequired().HasMaxLength(200);
                b.Property(e => e.Description).HasMaxLength(2000);
            });

            modelBuilder.Entity<EventReadModel>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Title).HasMaxLength(200);
            });
        }
    }
}
