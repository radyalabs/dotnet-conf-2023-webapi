using dotnetConf2023.Core.Abstractions;

namespace dotnetConf2023.Core.Identity.Commands.VerifyEmail;

public sealed class VerifyEmailCommandHandler : ICommandHandler<VerifyEmailCommand, Result>
{
    private readonly IUserService _userService;
    private readonly IDbContext _dbContext;
    private readonly IClock _clock;

    public VerifyEmailCommandHandler(IUserService userService, IDbContext dbContext, IClock clock)
    {
        _userService = userService;
        _dbContext = dbContext;
        _clock = clock;
    }

    /// <summary>
    /// Pseudo-code
    /// 1. Get user by email activation code
    /// 2. Then update
    /// 3. Return
    /// </summary>
    /// <param name="request">Request <see cref="VerifyEmailCommand"/></param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/></param>
    /// <returns>Plain <see cref="Result"/></returns>
    public async ValueTask<Result> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserByEmailActivationCodeAsync(request.Code, cancellationToken);
        if (user is null || user.EmailActivationStatus != EmailActivationStatus.NeedActivation)
            return Result.Failure(Error.Create("ExVE001", "Data not found."));

        user.EmailActivationCode = null;
        user.EmailActivationStatus = EmailActivationStatus.Activated;
        user.EmailActivationAt = _clock.CurrentDate();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}