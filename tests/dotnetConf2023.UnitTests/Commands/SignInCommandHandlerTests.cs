using dotnetConf2023.Core.Identity.Commands.SignIn;
using dotnetConf2023.Domain.Entities;
using dotnetConf2023.Shared.Abstraction.Auth;
using dotnetConf2023.UnitTests.Extensions;
using MockQueryable.Moq;
using Moq;

namespace dotnetConf2023.UnitTests.Commands;

public class SignInCommandHandlerTests
{
    [Fact]
    public async Task Handler_SignInCommandHandler_ShouldReturnSuccess()
    {
        //given
        var command = new SignInCommand
        {
            ClientId = Guid.NewGuid().ToString(),
            Username = "test",
            Password = "test"
        };

        var user = new User
        {
            Username = command.Username, NormalizedUsername = command.Username.ToUpper(),
            Password = "abcdef",
            LastPasswordChangeAt = DateTime.Now
        };
        user.UserRoles.Add(new UserRole { UserId = user.UserId, RoleId = "adm" });

        //Inject
        var userService = UserServiceBuilderExtensions.Create();
        userService.Setup(e => e.GetUserByUsernameFullAsync(command.Username, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        userService.Setup(e => e.VerifyPassword(user.Password, command.Password))
            .Returns(true);

        var clockService = ClockBuilderExtensions.Create();

        var dbContext = DbContextBuilderExtensions.Create();
        dbContext.Setup(e => e.Set<UserToken>())
            .Returns(new List<UserToken>().AsQueryable().BuildMockDbSet().Object);

        var authOptions = AuthOptionsBuilderExtensions.Create();

        var requestStorage = RequestStorageBuilderExtensions.Create();

        var authManager = AuthManagerBuilderExtensions.Create();
        authManager.Setup(e => e.CreateToken(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<string?>(),
                It.IsAny<IDictionary<string, IEnumerable<string>>?>()))
            .Returns(new JsonWebToken());

        var ctor = new SignInCommandHandler(
            userService.Object,
            clockService.Object,
            dbContext.Object,
            authOptions,
            requestStorage.Object,
            authManager.Object
        );

        user.UserTokens.Count.ShouldBe(0);

        var result = await ctor.Handle(command, CancellationToken.None);

        user.UserTokens.Count.ShouldBe(1);

        //Should
        result.IsSuccess.ShouldBeTrue();

        //Verify calling GetUserByUsernameFullAsync once
        userService.Verify(e =>
                e.GetUserByUsernameFullAsync(
                    command.Username,
                    It.IsAny<CancellationToken>()),
            Times.Once);

        //Verify calling VerifyPassword once
        userService.Verify(e =>
                e.VerifyPassword(
                    user.Password!,
                    command.Password),
            Times.Once);

        //Verify SaveChangesAsync once
        dbContext.Verify(e => e.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        //Verify calling set RequestStorage
        requestStorage.Verify(e => e.Set(It.IsAny<string>(), It.IsAny<UserIdentifier>(), It.IsAny<TimeSpan>()));
    }

    [Fact]
    public async Task Handler_SignInCommandHandler_ShouldReturnFailure_When_UserService_VerifyPassword_False()
    {
        //given
        var command = new SignInCommand
        {
            ClientId = Guid.NewGuid().ToString(),
            Username = "test",
            Password = "test"
        };

        var user = new User
        {
            Username = command.Username, NormalizedUsername = command.Username.ToUpper(),
            Password = "abcdef"
        };

        //Inject
        var userService = UserServiceBuilderExtensions.Create();
        userService.Setup(e => e.GetUserByUsernameFullAsync(command.Username, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        userService.Setup(e => e.VerifyPassword(user.Password, command.Password))
            .Returns(false);

        var clockService = ClockBuilderExtensions.Create();

        var dbContext = DbContextBuilderExtensions.Create();

        var authOptions = AuthOptionsBuilderExtensions.Create();

        var requestStorage = RequestStorageBuilderExtensions.Create();

        var authManager = AuthManagerBuilderExtensions.Create();

        var ctor = new SignInCommandHandler(
            userService.Object,
            clockService.Object,
            dbContext.Object,
            authOptions,
            requestStorage.Object,
            authManager.Object
        );

        var result = await ctor.Handle(command, CancellationToken.None);

        //Should
        result.IsFailure.ShouldBeTrue();

        //Verify
        userService.Verify(e =>
                e.GetUserByUsernameFullAsync(
                    command.Username,
                    It.IsAny<CancellationToken>()),
            Times.Once);
        userService.Verify(e =>
                e.VerifyPassword(
                    user.Password!,
                    command.Password),
            Times.Once);
    }

    [Fact]
    public async Task Handler_SignInCommandHandler_ShouldReturnFailure_When_UsernameNotFound()
    {
        //given
        var command = new SignInCommand
        {
            ClientId = Guid.NewGuid().ToString(),
            Username = "test",
            Password = "test"
        };

        //Inject
        var userService = UserServiceBuilderExtensions.Create();
        var clockService = ClockBuilderExtensions.Create();
        var dbContext = DbContextBuilderExtensions.Create();
        var authOptions = AuthOptionsBuilderExtensions.Create();
        var requestStorage = RequestStorageBuilderExtensions.Create();
        var authManager = AuthManagerBuilderExtensions.Create();

        var ctor = new SignInCommandHandler(
            userService.Object,
            clockService.Object,
            dbContext.Object,
            authOptions,
            requestStorage.Object,
            authManager.Object
        );

        var result = await ctor.Handle(command, CancellationToken.None);

        //Should
        result.IsFailure.ShouldBeTrue();

        //Verify
        userService.Verify(e =>
                e.GetUserByUsernameFullAsync(
                    command.Username,
                    It.IsAny<CancellationToken>()),
            Times.Once);
    }
}