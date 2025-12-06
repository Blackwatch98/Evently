using Evently.Domain.EventAggregate;

namespace Evently.Application.Abstractions
{
    public interface IEventRepository
    {
        Task AddAsync(Event evt, CancellationToken cancellationToken = default);
        Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
