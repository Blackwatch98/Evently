using Evently.Domain.EventAggregate;
using Evently.Domain.RegistrationAggregate;
using Evently.Infrastructure.Outbox;
using Evently.Infrastructure.Projections.ReadModels;
using Microsoft.EntityFrameworkCore;

namespace Evently.Infrastructure
{
    public class EventlyDbContext : DbContext
    {
        public DbSet<Event> Events => Set<Event>();
        public DbSet<EventReadModel> EventReadModels => Set<EventReadModel>();
        public DbSet<Registration> Registrations => Set<Registration>();
        public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

        public EventlyDbContext(DbContextOptions<EventlyDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
