using dotnetConf2023.Core.Abstractions;
using dotnetConf2023.Core.Identity.Commands.RefreshToken;
using dotnetConf2023.Core.Identity.Commands.SignIn;
using dotnetConf2023.Domain.Entities;
using dotnetConf2023.IntegrationTests.Fixtures;
using dotnetConf2023.Shared.Abstraction.Auth;
using dotnetConf2023.Shared.Abstraction.Databases;
using dotnetConf2023.Shared.Abstraction.Storage;
using dotnetConf2023.Shared.Abstraction.Time;
using dotnetConf2023.Shared.Infrastructure.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace dotnetConf2023.IntegrationTests.Handlers;

[Collection(Constant.ServiceCollectionDefaultName)]
public class RefreshTokenHandlerTests : IClassFixture<ServiceFixture>
{
    private readonly RefreshTokenCommandHandler _ctor;
    private readonly ServiceProvider _serviceProvider;

    public RefreshTokenHandlerTests(ServiceFixture serviceFixture)
    {
        _serviceProvider = serviceFixture.ServiceProvider;
        _ctor = new RefreshTokenCommandHandler(
            _serviceProvider.GetRequiredService<IClock>(),
            _serviceProvider.GetRequiredService<IDbContext>(),
            _serviceProvider.GetRequiredService<AuthOptions>(),
            _serviceProvider.GetRequiredService<IRequestStorage>(),
            _serviceProvider.GetRequiredService<IAuthManager>(),
            _serviceProvider.GetRequiredService<IUserService>());
    }

    /// <summary>
    /// 1. Sign-in first, to make a user has user token
    /// 2. Return jwt
    /// 3. Check user token exists
    /// 4. Use handlers
    /// 5. Check current user token and last token
    /// </summary>
    [Fact]
    public async Task RefreshToken_Should_Return_Success()
    {
        var dbContext = _serviceProvider.GetRequiredService<IDbContext>();

        var userToken = await dbContext.Set<UserToken>().Where(e => e.UserId == Guid.Empty)
            .FirstOrDefaultAsync(CancellationToken.None);
        userToken.ShouldBeNull();

        var signInHandler = new SignInCommandHandler(_serviceProvider.GetRequiredService<IUserService>(),
            _serviceProvider.GetRequiredService<IClock>(),
            _serviceProvider.GetRequiredService<IDbContext>(),
            _serviceProvider.GetRequiredService<AuthOptions>(),
            _serviceProvider.GetRequiredService<IRequestStorage>(),
            _serviceProvider.GetRequiredService<IAuthManager>());

        var signInCommand = new SignInCommand
        {
            ClientId = Guid.NewGuid().ToString(),
            Username = "admin",
            Password = "Qwerty@1234"
        };
        await signInHandler.Handle(signInCommand, CancellationToken.None);

        userToken = await dbContext.Set<UserToken>().Where(e => e.UserId == Guid.Empty)
            .FirstOrDefaultAsync(CancellationToken.None);
        userToken.ShouldNotBeNull();

        var refreshTokenCommand = new RefreshTokenCommand()
        {
            ClientId = signInCommand.ClientId,
            RefreshToken = userToken.RefreshToken
        };

        var result = await _ctor.Handle(refreshTokenCommand, CancellationToken.None);

        var userTokens = await dbContext.Set<UserToken>().Where(e => e.UserId == Guid.Empty)
            .OrderByDescending(e => e.CreatedDt)
            .ToListAsync(CancellationToken.None);
        userTokens.Count.ShouldBeGreaterThan(1);

        userTokens[0].IsUsed.ShouldBeFalse();
        userTokens[1].IsUsed.ShouldBeTrue();
        result.IsSuccess.ShouldBeTrue();
    }
}