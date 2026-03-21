using Evently.Domain.Aggregates.RegistrationAggregate;

namespace Evently.Application.Abstractions;

public interface IRegistrationRepository
{
    Task AddAsync(Registration registration, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid eventId, Guid userId, CancellationToken cancellationToken = default);
    Task<int> CountForEventAsync(Guid eventId, CancellationToken cancellationToken = default);
}
