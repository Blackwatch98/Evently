namespace Evently.Domain.Exceptions;

public sealed class UserTooYoungException : DomainException
{
    public int MinimumAge { get; }
    public int ActualAge { get; }

    public UserTooYoungException(int minimumAge, int actualAge)
        : base($"User must be at least {minimumAge} years old. Actual age: {actualAge}.")
    {
        MinimumAge = minimumAge;
        ActualAge = actualAge;
    }
}
