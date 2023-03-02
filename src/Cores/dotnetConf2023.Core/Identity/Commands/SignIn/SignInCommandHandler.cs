using dotnetConf2023.Core.Abstractions;

namespace dotnetConf2023.Core.Identity.Commands.SignIn;

/// <summary>
/// Handler that handle sign in logic.
/// </summary>
public sealed class SignInCommandHandler : ICommandHandler<SignInCommand, Result<JsonWebToken>>
{
    private readonly IUserService _userService;
    private readonly IClock _clock;
    private readonly IDbContext _dbContext;
    private readonly AuthOptions _authOptions;
    private readonly IRequestStorage _requestStorage;
    private readonly IAuthManager _authManager;

    public SignInCommandHandler(IUserService userService, IClock clock, IDbContext dbContext, AuthOptions authOptions,
        IRequestStorage requestStorage, IAuthManager authManager)
    {
        _userService = userService;
        _clock = clock;
        _dbContext = dbContext;
        _authOptions = authOptions;
        _requestStorage = requestStorage;
        _authManager = authManager;
    }

    /// <summary>
    /// Pseudo-code
    /// 1. Get user by username using <see cref="IUserService"/>
    /// 2. Verify password using <see cref="IUserService"/>
    /// 3. Database logic
    /// 4. Add token to mem-cached
    /// 5. Return
    /// </summary>
    /// <param name="request">See <see cref="SignInCommand"/></param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/></param>
    /// <returns>type of <see cref="Result{TValue}"/> with value of <see cref="JsonWebToken"/></returns>
    public async ValueTask<Result<JsonWebToken>> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserByUsernameFullAsync(request.Username, cancellationToken);
        if (user?.Password is null)
            return Result.Failure<JsonWebToken>(Error.Create("ExSI001", "Invalid username or password."));

        if (!_userService.VerifyPassword(user.Password!, request.Password))
            return Result.Failure<JsonWebToken>(Error.Create("ExSI002", "Invalid username or password."));

        var refreshToken = Guid.NewGuid().ToString("N");

        var newUserToken = new UserToken
        {
            UserId = user.UserId,
            ClientId = request.ClientId,
            RefreshToken = refreshToken,
            ExpiryAt = _clock.CurrentDate().Add(_authOptions.RefreshTokenExpiry),
            DeviceType = request.GetDeviceType()
        };

        user.UserTokens.Add(newUserToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        _requestStorage.Set($"{user.UserId}{request.ClientId}",
            new UserIdentifier
            {
                UserId = user.UserId, IdentifierId = refreshToken,
                LastChangePassword = user.LastPasswordChangeAt!.Value, TokenId = newUserToken.UserTokenId.ToString()
            }, _authOptions.Expiry);

        var claims = Extensions.GenerateCustomClaims(user, request.GetDeviceType());

        var jwt = _authManager.CreateToken(user.UserId, request.ClientId, refreshToken,
            newUserToken.UserTokenId.ToString(), role: null, audience: null,
            claims: claims);

        //jwt claims clear, for result only
        jwt.Claims.Clear();

        return Result.Success(jwt);
    }
}