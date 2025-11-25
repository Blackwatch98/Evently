using Evently.Domain.Abstractions;

namespace Evently.Domain.Events
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
