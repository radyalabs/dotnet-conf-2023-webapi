using dotnetConf2023.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace dotnetConf2023.UnitTests.Extensions;

public static class PasswordHasherUserBuilderExtensions
{
    public static Mock<IPasswordHasher<User>> Create()
    {
        var mock = new Mock<IPasswordHasher<User>>();
        return mock;
    }
}