namespace Evently.Api.Endpoints.Users.Contracts;

public sealed record CreateUserRequest(
    string FirstName,
    string LastName,
    int Age,
    string Email,
    string Password,
    int RoleId);
