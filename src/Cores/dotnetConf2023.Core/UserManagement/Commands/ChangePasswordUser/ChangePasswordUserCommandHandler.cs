using dotnetConf2023.Core.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace dotnetConf2023.Core.UserManagement.Commands.ChangePasswordUser;

public class ChangePasswordUserCommandHandler : ICommandHandler<ChangePasswordUserCommand, Result>
{
    private readonly IUserService _userService;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IDbContext _dbContext;

    public ChangePasswordUserCommandHandler(IUserService userService, IPasswordHasher<User> passwordHasher,
        IDbContext dbContext)
    {
        _userService = userService;
        _passwordHasher = passwordHasher;
        _dbContext = dbContext;
    }

    /// <summary>
    /// First get user object by user id, then directly update it`s password
    /// </summary>
    /// <param name="request">See <see cref="ChangePasswordUserCommand"/></param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/></param>
    /// <returns>See <see cref="Result"/></returns>
    public async ValueTask<Result> Handle(ChangePasswordUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return Result.Failure(Error.Create("ExCP001", "User not found."));

        user.Password = _passwordHasher.HashPassword(null!, request.NewPassword);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}