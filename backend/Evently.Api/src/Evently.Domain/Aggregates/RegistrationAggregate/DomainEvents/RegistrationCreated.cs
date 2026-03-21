using Evently.Domain.Common;

namespace Evently.Domain.Aggregates.RegistrationAggregate.DomainEvents;

public sealed record RegistrationCreated(
    Guid RegistrationId,
    Guid EventId,
    Guid UserId
) : IDomainEvent
{
    public DateTime OccuredAt { get; set; } = DateTime.UtcNow;
}
