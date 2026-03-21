using Evently.Domain.Aggregates.UserAggregate;

namespace Evently.Application.Abstractions.Authentication;

public interface IJwtProvider
{
    string Generate(User user);
    DateTime GetAccessTokenExpirationUtc();
}