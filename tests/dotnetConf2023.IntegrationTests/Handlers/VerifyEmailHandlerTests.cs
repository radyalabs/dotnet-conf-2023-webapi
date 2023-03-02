using dotnetConf2023.Core.Abstractions;
using dotnetConf2023.Core.Identity.Commands.ChangeEmail;
using dotnetConf2023.Core.Identity.Commands.VerifyEmail;
using dotnetConf2023.Domain.Entities.Enums;
using dotnetConf2023.IntegrationTests.Fixtures;
using dotnetConf2023.Shared.Abstraction.Databases;
using dotnetConf2023.Shared.Abstraction.Encryption;
using dotnetConf2023.Shared.Abstraction.Time;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace dotnetConf2023.IntegrationTests.Handlers;

[Collection(Constant.ServiceCollectionDefaultName)]
public class VerifyEmailHandlerTests : IClassFixture<ServiceFixture>
{
    private readonly VerifyEmailCommandHandler _ctor;
    private readonly ServiceProvider _serviceProvider;

    public VerifyEmailHandlerTests(ServiceFixture serviceFixture)
    {
        _serviceProvider = serviceFixture.ServiceProvider;
        _ctor = new VerifyEmailCommandHandler(
            serviceFixture.ServiceProvider.GetRequiredService<IUserService>(),
            serviceFixture.ServiceProvider.GetRequiredService<IDbContext>(),
            serviceFixture.ServiceProvider.GetRequiredService<IClock>());
    }

    [Fact]
    public async Task VerifyEmail_When_InvalidParameter_Should_Return_Success()
    {
        var changeEmailCommand = new ChangeEmailCommand
        {
            NewEmail = "test@test.com"
        };
        changeEmailCommand.SetUserId(Guid.Empty);
        var changeEmailHandler = new ChangeEmailCommandHandler(_serviceProvider.GetRequiredService<IUserService>(),
            _serviceProvider.GetRequiredService<IRng>(),
            _serviceProvider.GetRequiredService<IDbContext>());
        await changeEmailHandler.Handle(changeEmailCommand, CancellationToken.None);

        var userService = _serviceProvider.GetRequiredService<IUserService>();
        var user = (await userService.GetUserByIdAsync(Guid.Empty, CancellationToken.None))!;
        user.EmailActivationCode.ShouldNotBeNullOrEmpty();
        user.EmailActivationStatus.ShouldBe(EmailActivationStatus.NeedActivation);

        var verifyEmailCommand = new VerifyEmailCommand
        {
            Code = user.EmailActivationCode!
        };
        var result = await _ctor.Handle(verifyEmailCommand, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task VerifyEmail_When_InvalidParameter_Should_Return_Failed()
    {
        var verifyEmailCommand = new VerifyEmailCommand
        {
            Code = Guid.NewGuid().ToString()
        };
        var result = await _ctor.Handle(verifyEmailCommand, CancellationToken.None);
        result.IsFailure.ShouldBeTrue();
    }
}