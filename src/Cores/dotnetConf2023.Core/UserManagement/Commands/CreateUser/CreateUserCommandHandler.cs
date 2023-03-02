using dotnetConf2023.Core.Abstractions;
using dotnetConf2023.Domain;
using Microsoft.AspNetCore.Identity;

namespace dotnetConf2023.Core.UserManagement.Commands.CreateUser;

public sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Result>
{
    private readonly IUserService _userService;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IDbContext _dbContext;

    public CreateUserCommandHandler(IUserService userService, IPasswordHasher<User> passwordHasher,
        IDbContext dbContext)
    {
        _userService = userService;
        _passwordHasher = passwordHasher;
        _dbContext = dbContext;
    }

    /// <summary>
    /// First get user object by username, if exists or not null, throws error, then
    /// create new object user and save to database
    /// </summary>
    /// <param name="request">See <see cref="CreateUserCommand"/></param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/></param>
    /// <returns>See <see cref="Result"/></returns>
    public async ValueTask<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserByUsernameAsync(request.NormalizedUsername, cancellationToken);
        if (user is not null)
            return Result.Failure(Error.Create("ExCU001", "User already registered."));

        user = new User
        {
            Username = request.Username.ToLower(),
            NormalizedUsername = request.NormalizedUsername,
            FullName = request.Fullname,
            Password = _passwordHasher.HashPassword(default!, request.Password)
        };

        user.UserRoles.Add(new UserRole { RoleId = RoleConstant.User });

        _dbContext.Insert(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}