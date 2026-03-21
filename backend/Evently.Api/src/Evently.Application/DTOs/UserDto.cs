namespace Evently.Application.DTOs;

public sealed record UserDto(
    Guid IdUser,
    string FirstName,
    string LastName,
    int Age,
    string Email,
    bool IsBlocked,
    int RoleId,
    string RoleName,
    DateTime CreatedAt);
