namespace Evently.Application.Abstractions.Authentication;

public interface IRefreshTokenGenerator
{
    string Generate();
    DateTime GetRefreshTokenExpirationUtc();
}
