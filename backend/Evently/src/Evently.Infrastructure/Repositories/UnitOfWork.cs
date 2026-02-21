using Evently.Application.Abstractions;
using Evently.Domain.Common;

namespace Evently.Infrastructure.Repositories
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly EventlyDbContext _dbContext;
        private readonly IDomainEventDispatcher _domainEventDispatcher;
        public UnitOfWork(
            EventlyDbContext dbContext,
            IDomainEventDispatcher domainEventDispatcher)
        {
            _dbContext = dbContext;
            _domainEventDispatcher = domainEventDispatcher;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // 1. Zbieramy eventy domenowe z wszystkich encji
            var domainEvents = _dbContext.ChangeTracker
                .Entries<Entity>() // Entity z Evently.Domain.Abstractions
                .SelectMany(e => e.Entity.DomainEvents)
                .ToList();

            // 2. Czyścimy eventy na encjach (żeby nie odpaliły się drugi raz)
            foreach (var entry in _dbContext.ChangeTracker.Entries<Entity>())
            {
                entry.Entity.ClearDomainEvents();
            }

            // 3. Dispatchujemy eventy
            if (domainEvents.Count > 0)
            {
                await _domainEventDispatcher.DispatchAsync(domainEvents, cancellationToken);
            }

            // 4. Zapisujemy zmiany w bazie
            var result = await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
