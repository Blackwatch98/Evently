using Evently.Api.Endpoints.Auth.Contracts;
using Evently.Application.Features.Auth;
using System.Security.Claims;

namespace Evently.Api.Endpoints.Auth;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/auth")
            .WithTags("Auth");

        group.MapPost("/api/auth/login", async (
            LoginRequest request,
            LoginCommandHandler handler,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.Handle(
                new LoginCommand(
                    request.Email,
                    request.Password),
                cancellationToken);

            return Results.Ok(result);
        })
        .WithName("Login");

        group.MapPost("/api/auth/refresh", async (
            RefreshTokenRequest request,
            RefreshTokenCommandHandler handler,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.Handle(
                new RefreshTokenCommand(
                    request.RefreshToken),
                cancellationToken);

            return Results.Ok(result);
        })
        .WithName("RefreshToken");

        group.MapPost("/api/auth/logout", async (
            ClaimsPrincipal claimsPrincipal,
            LogoutCommandHandler handler,
            CancellationToken cancellationToken) =>
        {
            var userIdValue = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdValue, out var userId))
            {
                return Results.Unauthorized();
            }

            await handler.Handle(
                new LogoutCommand(userId),
                cancellationToken);

            return Results.NoContent();
        })
        .RequireAuthorization()
        .WithName("LogOut");

        return endpoints;
    }
}