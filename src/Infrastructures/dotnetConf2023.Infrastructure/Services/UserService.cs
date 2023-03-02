using dotnetConf2023.Core.Abstractions;
using dotnetConf2023.Core.Models;
using dotnetConf2023.Domain.Entities;
using dotnetConf2023.Shared.Abstraction.Databases;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace dotnetConf2023.Infrastructure.Services;

internal class UserService : IUserService
{
    private readonly IDbContext _dbContext;
    private readonly IPasswordHasher<User>? _passwordHasher;

    public UserService(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public UserService(IDbContext dbContext, IPasswordHasher<User> passwordHasher) : this(dbContext)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
    }

    public IQueryable<User> GetBaseQueryWithoutInclude()
        => _dbContext.Set<User>().AsQueryable();

    public IQueryable<User> GetBaseQuery()
        => _dbContext.Set<User>().Include(e => e.UserRoles).AsQueryable();

    public Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
        => GetBaseQuery().Where(e => e.UserId == userId).FirstOrDefaultAsync(cancellationToken);

    public Task<User?> GetUserByEmailActivationCodeAsync(string code, CancellationToken cancellationToken)
        => GetBaseQuery().Where(e => e.EmailActivationCode == code)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        username = username.ToUpperInvariant();
        return GetBaseQuery().Where(e => e.NormalizedUsername == username)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<User?> GetUserByUsernameFullAsync(string username, CancellationToken cancellationToken)
    {
        username = username.ToUpperInvariant();

        return GetBaseQuery()
            .Include(e => e.UserRoles)
            .Where(e => e.NormalizedUsername == username)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<User?> GetUserByUserIdFullAsync(Guid userId, CancellationToken cancellationToken)
        => GetBaseQuery()
            .Include(e => e.UserRoles)
            .Where(e => e.UserId == userId)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<UserCompactForLoginModel?> GetUserCompactForLoginAsync(Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await GetBaseQuery()
            .Where(e => e.UserId == userId)
            .Select(e => new UserCompactForLoginModel
            {
                UserId = e.UserId,
                Username = e.Username,
                LastPasswordChangeAt = e.LastPasswordChangeAt
            })
            .FirstOrDefaultAsync(cancellationToken);
        if (result is null) return null;

        result.Roles = await _dbContext.Set<UserRole>().Where(e => e.UserId == userId).Select(e => e.RoleId)
            .ToArrayAsync(cancellationToken);

        return result;
    }

    public bool VerifyPassword(string currentPassword, string password)
        => _passwordHasher?.VerifyHashedPassword(default!, currentPassword, password) ==
           PasswordVerificationResult.Success;

    public string HashPassword(string password)
        => _passwordHasher?.HashPassword(default!, password)!;
}