using Evently.Domain.EventAggregate;

namespace Evently.Application.Abstractions
{
    public interface IEventRepository
    {
        Task AddAsync(Event evt, CancellationToken cancellationToken = default);
    }
}
