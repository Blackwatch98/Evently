using Evently.Application.Abstractions;
using Evently.Domain.Aggregates.EventAggregate;
using Microsoft.EntityFrameworkCore;

namespace Evently.Infrastructure.Repositories;

public sealed class EventRepository : IEventRepository
{
    private readonly EventlyDbContext _dbContext;

    public EventRepository(EventlyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Event evt, CancellationToken cancellationToken = default)
        => await _dbContext.Events.AddAsync(evt, cancellationToken).AsTask();

    public async Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _dbContext.Events.FirstOrDefaultAsync(e => e.IdEvent == id, cancellationToken);
}
