using Evently.Application.Abstractions;
using Evently.Domain.Aggregates.UserAggregate;
using Microsoft.EntityFrameworkCore;

namespace Evently.Infrastructure.Repositories;

internal sealed class UserRepository : IUserRepository
{
    private readonly EventlyDbContext _dbContext;

    public UserRepository(EventlyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .Include(x => x.Role)
            .SingleOrDefaultAsync(x => x.IdUser == userId, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .Include(x => x.Role)
            .SingleOrDefaultAsync(
                x => x.Email == email,
                cancellationToken);
    }

    public async Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .Include(x => x.Role)
            .SingleOrDefaultAsync(
                x => x.RefreshToken == refreshToken,
                cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);
    }

    public Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        _dbContext.Users.Update(user);
        return Task.CompletedTask;
    }
}
