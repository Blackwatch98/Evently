using Evently.Application.Abstractions;
using Evently.Domain.RegistrationAggregate;

namespace Evently.Infrastructure.Repositories
{
    public sealed class RegistrationRepository : IRegistrationRepository
    {
        private readonly EventlyDbContext _dbContext;
        public RegistrationRepository(EventlyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task AddAsync(Registration registration, CancellationToken cancellationToken = default)
            => _dbContext.Registrations.AddAsync(registration, cancellationToken).AsTask();
    }
}
