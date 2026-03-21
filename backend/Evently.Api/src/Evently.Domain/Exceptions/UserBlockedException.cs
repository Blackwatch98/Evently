namespace Evently.Domain.Exceptions;

public sealed class UserBlockedException : DomainException
{
    public Guid UserId { get; }

    public UserBlockedException(Guid userId)
        : base($"User with id '{userId}' is blocked.")
    {
        UserId = userId;
    }
}