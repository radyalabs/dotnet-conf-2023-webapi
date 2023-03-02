using dotnetConf2023.Shared.Abstraction.Auth;
using Moq;

namespace dotnetConf2023.UnitTests.Extensions;

public static class AuthManagerBuilderExtensions
{
    public static Mock<IAuthManager> Create()
    {
        var mock = new Mock<IAuthManager>();
        return mock;
    }
}