using Evently.Application.Abstractions;
using Evently.Application.Abstractions.Authentication;
using Evently.Domain.DTOs;

namespace Evently.Application.Features.Auth;

public sealed class LoginCommandHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider,
        IRefreshTokenGenerator refreshTokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
        _refreshTokenGenerator = refreshTokenGenerator;
    }

    public async Task<TokenResponse> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(
            command.Email.Trim().ToLowerInvariant(),
            cancellationToken);

        if (user is null)
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        if (user.IsBlocked)
        {
            throw new UnauthorizedAccessException("User is blocked.");
        }

        if (!_passwordHasher.Verify(command.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        var accessToken = _jwtProvider.Generate(user);
        var accessTokenExpiresAtUtc = _jwtProvider.GetAccessTokenExpirationUtc();

        var refreshToken = _refreshTokenGenerator.Generate();
        var refreshTokenExpiresAtUtc = _refreshTokenGenerator.GetRefreshTokenExpirationUtc();

        user.SetRefreshToken(refreshToken, refreshTokenExpiresAtUtc);

        await _userRepository.UpdateAsync(user, cancellationToken);

        return new TokenResponse(
            accessToken,
            refreshToken,
            accessTokenExpiresAtUtc,
            refreshTokenExpiresAtUtc);
    }
}