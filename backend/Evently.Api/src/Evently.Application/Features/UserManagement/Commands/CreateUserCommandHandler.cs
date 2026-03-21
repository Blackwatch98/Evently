using Evently.Application.Abstractions;
using Evently.Application.Abstractions.Authentication;
using Evently.Domain.Aggregates.UserAggregate;

namespace Evently.Application.Features.UserManagement.Commands;

public sealed class CreateUserCommandHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> Handle(
        string firstName,
        string lastName,
        int age,
        string email,
        string password,
        int roleId,
        CancellationToken cancellationToken)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();

        var existingUser = await _userRepository.GetByEmailAsync(normalizedEmail, cancellationToken);

        if (existingUser is not null)
        {
            throw new InvalidOperationException("User with this email already exists.");
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password cannot be empty.", nameof(password));
        }

        var passwordHash = _passwordHasher.Hash(password);

        var user = User.Create(
            firstName,
            lastName,
            age,
            normalizedEmail,
            passwordHash,
            roleId);

        await _userRepository.AddAsync(user, cancellationToken);

        return user.IdUser;
    }
}
