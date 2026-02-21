using Evently.Domain.Common;

namespace Evently.Domain.RegistrationAggregate.DomainEvents
{
    public sealed record ParticipantRegistered(
        Guid RegistrationId,
        Guid EventId,
        string Email
    ) : IDomainEvent
    {
        public DateTime OccuredAt { get; set; } = DateTime.UtcNow;
    }
}
