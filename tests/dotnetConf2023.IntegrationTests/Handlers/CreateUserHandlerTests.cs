using dotnetConf2023.Core.Abstractions;
using dotnetConf2023.Core.UserManagement.Commands.CreateUser;
using dotnetConf2023.Domain.Entities;
using dotnetConf2023.IntegrationTests.Fixtures;
using dotnetConf2023.Shared.Abstraction.Databases;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace dotnetConf2023.IntegrationTests.Handlers;

[Collection(Constant.ServiceCollectionDefaultName)]
public class CreateUserHandlerTests : IClassFixture<ServiceFixture>
{
    private readonly CreateUserCommandHandler _ctor;
    private readonly ServiceProvider _serviceProvider;

    public CreateUserHandlerTests(ServiceFixture serviceFixture)
    {
        _serviceProvider = serviceFixture.ServiceProvider;
        _ctor = new CreateUserCommandHandler(
            serviceFixture.ServiceProvider.GetRequiredService<IUserService>(),
            serviceFixture.ServiceProvider.GetRequiredService<IPasswordHasher<User>>(),
            serviceFixture.ServiceProvider.GetRequiredService<IDbContext>());
    }

    [Fact]
    public async Task CreateUser_When_Username_Already_Registered_Should_Return_Failed()
    {
        var createUserCommand = new CreateUserCommand
        {
            Username = "admin",
            Fullname = "Loree Ipsum",
            Password = "Qwerty1234"
        };

        var result = await _ctor.Handle(createUserCommand, CancellationToken.None);
        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldBe("ExCU001");
    }

    [Fact]
    public async Task CreateUser_When_Username_Already_Registered_Should_Return_Success()
    {
        var createUserCommand = new CreateUserCommand
        {
            Username = "testuser",
            Fullname = "Loree Ipsum",
            Password = "Qwerty1234"
        };

        var result = await _ctor.Handle(createUserCommand, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();

        var userService = _serviceProvider.GetRequiredService<IUserService>();
        var user = await userService.GetUserByUsernameAsync(createUserCommand.Username, CancellationToken.None);

        user.ShouldNotBeNull();
    }
}