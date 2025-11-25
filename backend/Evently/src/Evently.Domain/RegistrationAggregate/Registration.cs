using Evently.Domain.Abstractions;
using Evently.Domain.Events;

namespace Evently.Domain.RegistrationAggregate
{
    public sealed class Registration : Entity
    {
        public Guid IdRegistration { get; private set; }
        public Guid EventId { get; private set; }
        public string Email { get; private set; } = default!;
        public DateTime RegisteredAt { get; private set; }

        private Registration() { }

        private Registration(Guid eventId, string email)
        {
            IdRegistration = Guid.NewGuid();
            EventId = eventId;
            Email = email;
            RegisteredAt = DateTime.UtcNow;

            AddDomainEvent(new ParticipantRegistered(IdRegistration, EventId, Email));
        }

        public static Registration Create(Guid eventId, string email)
            => new(eventId, email);
    }
}
