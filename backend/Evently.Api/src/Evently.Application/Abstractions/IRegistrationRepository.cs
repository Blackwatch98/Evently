using Evently.Domain.RegistrationAggregate;

namespace Evently.Application.Abstractions;

public interface IRegistrationRepository
{
    Task AddAsync(Registration registration, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid eventId, string email, CancellationToken cancellationToken = default);
    Task<int> CountForEventAsync(Guid eventId, CancellationToken cancellationToken = default);
}
