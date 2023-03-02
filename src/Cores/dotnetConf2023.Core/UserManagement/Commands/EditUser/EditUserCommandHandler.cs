using dotnetConf2023.Core.Abstractions;

namespace dotnetConf2023.Core.UserManagement.Commands.EditUser;

public class EditUserCommandHandler : ICommandHandler<EditUserCommand, Result>
{
    private readonly IUserService _userService;
    private readonly IDbContext _dbContext;

    public EditUserCommandHandler(IUserService userService, IDbContext dbContext)
    {
        _userService = userService;
        _dbContext = dbContext;
    }

    /// <summary>
    /// First get user object by user id, then update its property based on request.
    /// </summary>
    /// <param name="request">See <see cref="EditUserCommand"/></param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/></param>
    /// <returns>See <see cref="Result"/></returns>
    public async ValueTask<Result> Handle(EditUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return Result.Failure(Error.Create("ExEU001", "Data not found."));

        if (request.FullName.IsNotNullOrWhiteSpace())
            if (user.FullName != request.FullName)
                user.FullName = request.FullName!;

        if (request.AboutMe.IsNotNullOrWhiteSpace())
            if (user.AboutMe != request.AboutMe)
                user.AboutMe = request.AboutMe;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}