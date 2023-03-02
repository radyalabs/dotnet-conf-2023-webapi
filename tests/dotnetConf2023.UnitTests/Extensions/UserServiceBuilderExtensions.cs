using dotnetConf2023.Core.Abstractions;
using Moq;

namespace dotnetConf2023.UnitTests.Extensions;

public static class UserServiceBuilderExtensions
{
    public static Mock<IUserService> Create()
    {
        var mock = new Mock<IUserService>();
        return mock;
    }
}