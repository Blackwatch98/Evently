using Evently.Domain.RegistrationAggregate;

namespace Evently.Application.Abstractions
{
    public interface IRegistrationRepository
    {
        Task AddAsync(Registration registration, CancellationToken cancellationToken = default);
    }
}
