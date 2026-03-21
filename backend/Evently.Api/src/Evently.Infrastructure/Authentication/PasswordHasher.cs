using Evently.Application.Abstractions.Authentication;
using Evently.Domain.Aggregates.UserAggregate;
using Microsoft.AspNetCore.Identity;

namespace Evently.Infrastructure.Authentication;

internal sealed class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<User> _passwordHasher = new();

    public string Hash(string password)
    {
        var fakeUser = (User?)null;
        return _passwordHasher.HashPassword(fakeUser!, password);
    }

    public bool Verify(string password, string passwordHash)
    {
        var fakeUser = (User?)null;

        var result = _passwordHasher.VerifyHashedPassword(
            fakeUser!,
            passwordHash,
            password);

        return result == PasswordVerificationResult.Success ||
               result == PasswordVerificationResult.SuccessRehashNeeded;
    }
}