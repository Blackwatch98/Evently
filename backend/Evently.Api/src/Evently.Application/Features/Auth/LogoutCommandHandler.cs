using Evently.Application.Abstractions;

namespace Evently.Application.Features.Auth;

public sealed class LogoutCommandHandler
{
    private readonly IUserRepository _userRepository;

    public LogoutCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Handle(LogoutCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(command.UserId, cancellationToken);

        if (user is null)
        {
            return;
        }

        user.ClearRefreshToken();

        await _userRepository.UpdateAsync(user, cancellationToken);
    }
}
