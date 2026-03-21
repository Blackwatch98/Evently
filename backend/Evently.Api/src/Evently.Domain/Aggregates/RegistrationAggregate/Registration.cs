using Evently.Domain.Aggregates.RegistrationAggregate.DomainEvents;
using Evently.Domain.Common;
using Evently.Domain.Enums;

namespace Evently.Domain.Aggregates.RegistrationAggregate;

public sealed class Registration : Entity
{
    public Guid IdRegistration { get; private set; }
    public Guid EventId { get; private set; }
    public Guid UserId { get; private set; }
    public RegistrationStatus Status { get; private set; }
    public DateTime RegisteredAt { get; private set; }

    private Registration() { }

private Registration(Guid eventId, Guid userId)
    {
        if (eventId == Guid.Empty)
        {
            throw new ArgumentException("EventId cannot be empty.", nameof(eventId));
        }

        if (userId == Guid.Empty)
        {
            throw new ArgumentException("UserId cannot be empty.", nameof(userId));
        }

        IdRegistration = Guid.NewGuid();
        EventId = eventId;
        UserId = userId;
        RegisteredAt = DateTime.UtcNow;

        AddDomainEvent(new RegistrationCreated(IdRegistration, EventId, UserId));
    }

    public static Registration Create(Guid eventId, Guid userId)
        => new(eventId, userId);
}
