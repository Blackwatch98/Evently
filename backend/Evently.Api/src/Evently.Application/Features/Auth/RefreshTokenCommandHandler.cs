using Evently.Application.Abstractions;
using Evently.Application.Abstractions.Authentication;
using Evently.Domain.DTOs;

namespace Evently.Application.Features.Auth;

public sealed class RefreshTokenCommandHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;

    public RefreshTokenCommandHandler(
        IUserRepository userRepository,
        IJwtProvider jwtProvider,
        IRefreshTokenGenerator refreshTokenGenerator)
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
        _refreshTokenGenerator = refreshTokenGenerator;
    }

    public async Task<TokenResponse> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByRefreshTokenAsync(command.RefreshToken, cancellationToken);

        if (user is null)
        {
            throw new UnauthorizedAccessException("Invalid refresh token.");
        }

        if (user.IsBlocked)
        {
            throw new UnauthorizedAccessException("User is blocked.");
        }

        if (user.RefreshToken != command.RefreshToken)
        {
            throw new UnauthorizedAccessException("Invalid refresh token.");
        }

        if (!user.RefreshTokenExpiresAtUtc.HasValue ||
            user.RefreshTokenExpiresAtUtc.Value <= DateTime.UtcNow)
        {
            throw new UnauthorizedAccessException("Refresh token expired.");
        }

        var accessToken = _jwtProvider.Generate(user);
        var accessTokenExpiresAtUtc = _jwtProvider.GetAccessTokenExpirationUtc();

        var newRefreshToken = _refreshTokenGenerator.Generate();
        var newRefreshTokenExpiresAtUtc = _refreshTokenGenerator.GetRefreshTokenExpirationUtc();

        user.SetRefreshToken(newRefreshToken, newRefreshTokenExpiresAtUtc);

        await _userRepository.UpdateAsync(user, cancellationToken);

        return new TokenResponse(
            accessToken,
            newRefreshToken,
            accessTokenExpiresAtUtc,
            newRefreshTokenExpiresAtUtc);
    }
}
