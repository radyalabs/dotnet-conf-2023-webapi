using dotnetConf2023.Core.Abstractions;

namespace dotnetConf2023.Core.Identity.Commands.RefreshToken;

public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, Result<JsonWebToken>>
{
    private readonly IClock _clock;
    private readonly IDbContext _dbContext;
    private readonly AuthOptions _authOptions;
    private readonly IRequestStorage _requestStorage;
    private readonly IAuthManager _authManager;
    private readonly IUserService _userService;

    public RefreshTokenCommandHandler(IClock clock, IDbContext dbContext,
        AuthOptions authOptions, IRequestStorage requestStorage, IAuthManager authManager, IUserService userService)
    {
        _clock = clock;
        _dbContext = dbContext;
        _authOptions = authOptions;
        _requestStorage = requestStorage;
        _authManager = authManager;
        _userService = userService;
    }

    /// <summary>
    /// Pseudo-code
    /// 1. Get UserToken by params
    /// 2. Create new user token and refresh token
    /// 3. Database logic
    /// 4. Add to mem cached
    /// 5. Return
    /// </summary>
    /// <param name="request">See <see cref="RefreshTokenCommand"/></param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/></param>
    /// <returns>type of <see cref="Result{TValue}"/> with value of <see cref="JsonWebToken"/></returns>
    public async ValueTask<Result<JsonWebToken>> Handle(RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var userToken = await _dbContext.Set<UserToken>()
            .Where(e => e.ClientId == request.ClientId && e.RefreshToken == request.RefreshToken)
            .FirstOrDefaultAsync(cancellationToken);

        if (userToken is null || userToken.IsUsed || userToken.ExpiryAt < _clock.CurrentDate())
            return Result.Failure<JsonWebToken>(Error.Create("ExRT001", "Invalid request."));

        userToken.IsUsed = true;

        var refreshToken = Guid.NewGuid().ToString("N");

        var newUserToken = new UserToken
        {
            UserId = userToken.UserId,
            ClientId = request.ClientId,
            RefreshToken = refreshToken,
            ExpiryAt = _clock.CurrentDate().Add(_authOptions.RefreshTokenExpiry),
            DeviceType = userToken.DeviceType
        };

        userToken.User!.UserTokens.Add(newUserToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        var user = (await _userService.GetUserCompactForLoginAsync(userToken.UserId, cancellationToken))!;

        _requestStorage.Set($"{userToken.UserId}{request.ClientId}",
            new UserIdentifier
            {
                UserId = user.UserId, IdentifierId = refreshToken,
                LastChangePassword = user.LastPasswordChangeAt!.Value,
                TokenId = newUserToken.UserTokenId.ToString()
            }, _authOptions.Expiry);

        var claims =
            Extensions.GenerateCustomClaims(user.UserId.ToString(), user.Username, userToken.DeviceType, user.Roles);

        var jwt = _authManager.CreateToken(user.UserId, request.ClientId, refreshToken,
            newUserToken.UserTokenId.ToString(), role: null,
            audience: null,
            claims: claims);

        //jwt claims clear, for result only
        jwt.Claims.Clear();

        return Result.Success(jwt);
    }
}