using Evently.Application.Abstractions;
using Evently.Domain.EventAggregate;

namespace Evently.Infrastructure.Repositories
{
    public sealed class EventRepository : IEventRepository
    {
        private readonly EventlyDbContext _dbContext;

        public EventRepository(EventlyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Event evt, CancellationToken cancellationToken = default)
        {
            await _dbContext.Events.AddAsync(evt, cancellationToken);
        }
    }
}
