using Evently.Application.Abstractions.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace Evently.Infrastructure.Authentication;

internal sealed class RefreshTokenGenerator : IRefreshTokenGenerator
{
    private readonly JwtOptions _options;

    public RefreshTokenGenerator(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public string Generate()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(randomBytes);
    }

    public DateTime GetRefreshTokenExpirationUtc()
        => DateTime.UtcNow.AddDays(_options.RefreshTokenExpirationInDays);
}
