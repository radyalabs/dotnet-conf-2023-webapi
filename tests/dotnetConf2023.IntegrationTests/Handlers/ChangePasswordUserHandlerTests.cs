using dotnetConf2023.Core.Abstractions;
using dotnetConf2023.Core.UserManagement.Commands.ChangePasswordUser;
using dotnetConf2023.Domain.Entities;
using dotnetConf2023.IntegrationTests.Fixtures;
using dotnetConf2023.Shared.Abstraction.Databases;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace dotnetConf2023.IntegrationTests.Handlers;

[Collection(Constant.ServiceCollectionDefaultName)]
public class ChangePasswordUserHandlerTests : IClassFixture<ServiceFixture>
{
    private readonly ChangePasswordUserCommandHandler _ctor;
    private readonly IUserService _userService;

    public ChangePasswordUserHandlerTests(ServiceFixture serviceFixture)
    {
        _userService = serviceFixture.ServiceProvider.GetRequiredService<IUserService>();
        _ctor = new ChangePasswordUserCommandHandler(
            serviceFixture.ServiceProvider.GetRequiredService<IUserService>(),
            serviceFixture.ServiceProvider.GetRequiredService<IPasswordHasher<User>>(),
            serviceFixture.ServiceProvider.GetRequiredService<IDbContext>());
    }

    [Fact]
    public async Task ChangePasswordUser_When_InvalidParameter_Should_Return_Failed()
    {
        var command = new ChangePasswordUserCommand
        {
            UserId = Guid.NewGuid(),
            NewPassword = "aabbccddee"
        };

        var result = await _ctor.Handle(command, CancellationToken.None);
        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldBe("ExCP001");
    }

    [Fact]
    public async Task ChangePasswordUser_When_CorrectParameter_Should_Return_Success()
    {
        var command = new ChangePasswordUserCommand
        {
            UserId = Guid.Empty,
            NewPassword = "Qwerty@1234567890"
        };

        var result = await _ctor.Handle(command, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();

        var user = await _userService.GetUserByIdAsync(command.UserId, CancellationToken.None);
        user.ShouldNotBeNull();
        _userService.VerifyPassword(user.Password!, command.NewPassword).ShouldBeTrue();
    }
}