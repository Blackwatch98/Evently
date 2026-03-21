using Evently.Application.Abstractions;
using Evently.Application.DTOs;

namespace Evently.Application.Features.UserManagement.Queries;

public sealed class GetUsersQueryHandler
{
    private readonly IUserRepository _userRepository;

    public GetUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<UserDto>> Handle(CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);

        return users
            .Select(x => new UserDto(
                x.IdUser,
                x.FirstName,
                x.LastName,
                x.Age,
                x.Email,
                x.IsBlocked,
                x.RoleId,
                x.Role?.Name ?? string.Empty,
                x.CreatedAt))
            .ToList();
    }
}