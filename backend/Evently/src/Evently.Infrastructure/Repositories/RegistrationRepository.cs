using Evently.Application.Abstractions;
using Evently.Domain.RegistrationAggregate;
using Microsoft.EntityFrameworkCore;

namespace Evently.Infrastructure.Repositories
{
    public sealed class RegistrationRepository : IRegistrationRepository
    {
        private readonly EventlyDbContext _dbContext;
        public RegistrationRepository(EventlyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Registration registration, CancellationToken cancellationToken = default)
            => await _dbContext.Registrations.AddAsync(registration, cancellationToken).AsTask();

        public async Task<bool> ExistsAsync(Guid eventId, string email, CancellationToken cancellationToken = default)
            => await _dbContext.Registrations
                .AnyAsync(r => r.EventId == eventId && r.Email == email, cancellationToken);

        public async Task<int> CountForEventAsync(Guid eventId, CancellationToken cancellationToken = default)
            => await _dbContext.Registrations
                .CountAsync(r => r.EventId == eventId, cancellationToken);
    }
}
