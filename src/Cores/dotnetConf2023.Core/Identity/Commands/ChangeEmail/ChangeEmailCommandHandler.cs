using dotnetConf2023.Core.Abstractions;

namespace dotnetConf2023.Core.Identity.Commands.ChangeEmail;

public class ChangeEmailCommandHandler : ICommandHandler<ChangeEmailCommand, Result>
{
    private readonly IUserService _userService;
    private readonly IRng _rng;
    private readonly IDbContext _dbContext;

    public ChangeEmailCommandHandler(IUserService userService, IRng rng, IDbContext dbContext)
    {
        _userService = userService;
        _rng = rng;
        _dbContext = dbContext;
    }

    /// <summary>
    /// First get user by user id, then check if email is not same as current email,
    /// then update and also set user email activation status to be requested
    /// </summary>
    /// <param name="request">See <see cref="ChangeEmailCommand"/></param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/></param>
    /// <returns>See <see cref="Result"/></returns>
    public async ValueTask<Result> Handle(ChangeEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserByIdAsync(request.GetUserId()!.Value, cancellationToken);

        if (user is null)
            return Result.Failure(Error.Create("ExCE001", "User not found."));

        if (user.Email == request.NewEmail)
            return Result.Failure(Error.Create("ExCE002", "Email same as before."));

        user.Email = request.NewEmail;
        user.EmailActivationAt = null;
        user.EmailActivationCode = _rng.Generate(512);
        user.EmailActivationStatus = EmailActivationStatus.NeedActivation;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}