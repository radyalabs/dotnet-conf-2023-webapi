using dotnetConf2023.Core.Abstractions;
using dotnetConf2023.Core.UserManagement.Commands.EditUser;
using dotnetConf2023.IntegrationTests.Fixtures;
using dotnetConf2023.Shared.Abstraction.Databases;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace dotnetConf2023.IntegrationTests.Handlers;

[Collection(Constant.ServiceCollectionDefaultName)]
public class EditUserHandlerTests : IClassFixture<ServiceFixture>
{
    private readonly EditUserCommandHandler _ctor;
    private readonly IUserService _userService;

    public EditUserHandlerTests(ServiceFixture serviceFixture)
    {
        _userService = serviceFixture.ServiceProvider.GetRequiredService<IUserService>();
        _ctor = new EditUserCommandHandler(serviceFixture.ServiceProvider.GetRequiredService<IUserService>(),
            serviceFixture.ServiceProvider.GetRequiredService<IDbContext>());
    }

    [Fact]
    public async Task EditUser_When_Passed_Random_UserId_Should_Return_Failed()
    {
        var editUserCommand = new EditUserCommand
        {
            UserId = Guid.NewGuid(),
            FullName = "Loree Ipsum",
            AboutMe = "Im a software engineer"
        };

        var result = await _ctor.Handle(editUserCommand, CancellationToken.None);
        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldBe("ExEU001");
    }

    [Fact]
    public async Task EditUser_When_Passed_Correct_Parameters_Should_Return_Success()
    {
        var editUserCommand = new EditUserCommand
        {
            UserId = Guid.Empty,
            FullName = "Loree Ipsum",
            AboutMe = "Im a software engineer"
        };

        var result = await _ctor.Handle(editUserCommand, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();

        var user = await _userService.GetUserByIdAsync(editUserCommand.UserId, CancellationToken.None);
        user.ShouldNotBeNull();
        user.FullName.ShouldBe(editUserCommand.FullName);
        user.AboutMe.ShouldBe(editUserCommand.AboutMe);
    }
}