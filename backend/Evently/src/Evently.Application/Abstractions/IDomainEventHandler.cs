using Evently.Domain.Abstractions;

namespace Evently.Application.Abstractions
{
    public interface IDomainEventHandler<TEvent> where TEvent : IDomainEvent
    {
        Task Handle(TEvent domainEvent, CancellationToken cancellationToken = default);
    }
}
