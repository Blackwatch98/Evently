using Evently.Domain.Common;
using Evently.Domain.Entities;
using Evently.Domain.Exceptions;

namespace Evently.Domain.Aggregates.UserAggregate;

public sealed class User : Entity
{
    public Guid IdUser { get; private set; }
    public string FirstName { get; private set; } = default!;
    public string LastName { get; private set; } = default!;
    public int Age { get; private set; }
    public string Email { get; private set; } = default!;
    public bool IsBlocked { get; private set; }
    public int RoleId { get; private set; }
    public string? RefreshToken { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Role? Role { get; private set; }

    private User() {}
    private User(
            string firstName,
            string lastName,
            int age,
            string email,
            int roleId)
    {
        IdUser = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Age = age;
        Email = email;
        RoleId = roleId;
        IsBlocked = false;
        CreatedAt = DateTime.UtcNow;
        RefreshToken = null;

        // AddDomainEvent(new UserCreatedDomainEvent(IdUser, Email));
    }

    public static User Create(
        string firstName,
        string lastName,
        int age,
        string email,
        int roleId)
    {
        ValidateFirstName(firstName);
        ValidateLastName(lastName);
        ValidateAge(age);
        ValidateEmail(email);
        ValidateRole(roleId);

        return new User(firstName.Trim(), lastName.Trim(), age, email.Trim().ToLowerInvariant(), roleId);
    }

    public void UpdatePersonalData(string firstName, string lastName, int age)
    {
        ThrowIfBlocked();

        ValidateFirstName(firstName);
        ValidateLastName(lastName);
        ValidateAge(age);

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        Age = age;
    }

    public void ChangeEmail(string email)
    {
        ThrowIfBlocked();

        ValidateEmail(email);
        Email = email.Trim().ToLowerInvariant();
    }

    public void ChangeRole(int roleId)
    {
        ThrowIfBlocked();

        ValidateRole(roleId);
        RoleId = roleId;
    }

    public void SetRefreshToken(string refreshToken)
    {
        ThrowIfBlocked();

        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            throw new ArgumentException("Refresh token cannot be empty.", nameof(refreshToken));
        }

        RefreshToken = refreshToken;
    }

    public void ClearRefreshToken()
    {
        RefreshToken = null;
    }

    public void Block()
    {
        if (IsBlocked)
        {
            return;
        }

        IsBlocked = true;
    }

    public void Unblock()
    {
        if (!IsBlocked)
        {
            return;
        }

        IsBlocked = false;
    }

    private void ThrowIfBlocked()
    {
        if (IsBlocked)
        {
            throw new UserBlockedException(IdUser);
        }
    }

    private static void ValidateFirstName(string firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new ArgumentException("First name cannot be empty.", nameof(firstName));
        }

        if (firstName.Length > 100)
        {
            throw new ArgumentException("First name is too long.", nameof(firstName));
        }
    }

    private static void ValidateLastName(string lastName)
    {
        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException("Last name cannot be empty.", nameof(lastName));
        }

        if (lastName.Length > 100)
        {
            throw new ArgumentException("Last name is too long.", nameof(lastName));
        }
    }

    private static void ValidateAge(int age)
    {
        if (age < UserRules.MinimumRegistrationAge)
        {
            throw new UserTooYoungException(UserRules.MinimumRegistrationAge, age);
        }
    }

    private static void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email cannot be empty.", nameof(email));
        }

        if (email.Length > 256)
        {
            throw new ArgumentException("Email is too long.", nameof(email));
        }

        if (!email.Contains('@'))
        {
            throw new ArgumentException("Email has invalid format.", nameof(email));
        }
    }

    private static void ValidateRole(int roleId)
    {
        if (roleId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(roleId), "RoleId must be greater than zero.");
        }
    }
}
