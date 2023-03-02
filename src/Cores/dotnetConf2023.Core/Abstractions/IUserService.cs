namespace dotnetConf2023.Core.Abstractions;

public interface IUserService
{
    /// <summary>
    /// Get user object by id.
    /// </summary>
    /// <param name="userId">type of <see cref="Guid"/></param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/></param>
    /// <returns>See <see cref="User"/></returns>
    Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Get user object by email activation code.
    /// </summary>
    /// <param name="code">string</param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/></param>
    /// <returns>See <see cref="User"/></returns>
    Task<User?> GetUserByEmailActivationCodeAsync(string code, CancellationToken cancellationToken);

    /// <summary>
    /// Get user object by username. 
    /// </summary>
    /// <param name="username">string</param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/></param>
    /// <returns>See <see cref="User"/></returns>
    Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken);

    /// <summary>
    /// Get whole user by username, full meaning to gets deep into role, role modules, its children and permissions.
    /// </summary>
    /// <param name="username">string</param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/></param>
    /// <returns>See <see cref="User"/></returns>
    Task<User?> GetUserByUsernameFullAsync(string username, CancellationToken cancellationToken);

    /// <summary>
    /// Get user by user id, full meaning to gets deep into role, role modules, its children and permissions.
    /// </summary>
    /// <param name="userId">type of <see cref="Guid"/></param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/></param>
    /// <returns>See <see cref="User"/></returns>
    Task<User?> GetUserByUserIdFullAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Get user compact by user id, usually only for read data and high performance result.
    /// </summary>
    /// <param name="userId">type of <see cref="Guid"/></param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/></param>
    /// <returns>See <see cref="UserCompactForLoginModel"/></returns>
    Task<UserCompactForLoginModel?> GetUserCompactForLoginAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Verify password.
    /// </summary>
    /// <param name="currentPassword">string</param>
    /// <param name="password">string</param>
    /// <returns>bool</returns>
    bool VerifyPassword(string currentPassword, string password);

    /// <summary>
    /// Hashing password.
    /// </summary>
    /// <param name="password">string</param>
    /// <returns>string</returns>
    string HashPassword(string password);
}