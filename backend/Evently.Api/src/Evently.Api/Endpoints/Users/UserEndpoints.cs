using Evently.Api.Endpoints.Users.Contracts;
using Evently.Application.Features.UserManagement.Commands;
using Evently.Application.Features.UserManagement.Queries;
using Microsoft.AspNetCore.Mvc;
namespace Evently.Api.Endpoints.Users;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/users");

        group.MapPost("/", CreateUser);
        group.MapGet("/", GetUsers);

        return app;
    }

    private static async Task<IResult> CreateUser(
        CreateUserRequest request,
        [FromServices] CreateUserCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var userId = await handler.Handle(
            request.FirstName,
            request.LastName,
            request.Age,
            request.Email,
            request.Password,
            request.RoleId,
            cancellationToken);

        return Results.Created($"/api/users/{userId}", new { Id = userId });
    }

    private static async Task<IResult> GetUsers(
        [FromServices] GetUsersQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var users = await handler.Handle(cancellationToken);
        return Results.Ok(users);
    }
}
