using Evently.Application.Abstractions;
using Evently.Domain.Events;
using Evently.Infrastructure;
using Evently.Infrastructure.ReadModels;
using Microsoft.EntityFrameworkCore;

namespace Evently.Application.Events.EventCreatedHandlers
{
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
                .AnyAsync(x => x.Id == domainEvent.EventId, cancellationToken);

            if (exists)
                return;

            var readModel = new EventReadModel
            {
                Id = domainEvent.EventId,
                Title = domainEvent.Title,
                ScheduledAt = domainEvent.ScheduledAt
            };

            await _dbContext.EventReadModels.AddAsync(readModel, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
