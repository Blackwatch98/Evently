using Evently.Application.Abstractions;
using Evently.Domain.Aggregates.EventAggregate.DomainEvents;
using Evently.Infrastructure.Projections.ReadModels;
using Microsoft.EntityFrameworkCore;

namespace Evently.Infrastructure.Projections.Events;

public sealed class CreateEventReadModelHandler : IDomainEventHandler<EventCreated>
{
    private readonly EventlyDbContext _dbContext;

    public CreateEventReadModelHandler(EventlyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(EventCreated domainEvent, CancellationToken cancellationToken = default)
    {
        var exists = await _dbContext.EventReadModels
            .AnyAsync(x => x.IdReadModel == domainEvent.EventId, cancellationToken);

        if (exists)
            return;

        var readModel = new EventReadModel
        {
            IdReadModel = domainEvent.EventId,
            Title = domainEvent.Title,
            ScheduledAt = domainEvent.ScheduledAt
        };

        await _dbContext.EventReadModels.AddAsync(readModel, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
