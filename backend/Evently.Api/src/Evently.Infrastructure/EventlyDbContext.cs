using Evently.Domain.Aggregates.EventAggregate;
using Evently.Domain.Aggregates.RegistrationAggregate;
using Evently.Domain.Aggregates.UserAggregate;
using Evently.Domain.Entities;
using Evently.Infrastructure.Outbox;
using Evently.Infrastructure.Projections.ReadModels;
using Microsoft.EntityFrameworkCore;

namespace Evently.Infrastructure;

public class EventlyDbContext : DbContext
{
    public DbSet<Event> Events => Set<Event>();
    public DbSet<EventReadModel> EventReadModels => Set<EventReadModel>();
    public DbSet<Registration> Registrations => Set<Registration>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    public EventlyDbContext(DbContextOptions<EventlyDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
