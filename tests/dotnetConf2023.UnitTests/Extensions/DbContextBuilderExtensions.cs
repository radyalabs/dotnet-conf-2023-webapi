using dotnetConf2023.Shared.Abstraction.Databases;
using Moq;

namespace dotnetConf2023.UnitTests.Extensions;

public static class DbContextBuilderExtensions
{
    public static Mock<IDbContext> Create()
    {
        var mockDbContext = new Mock<IDbContext>();
        return mockDbContext;
    }
}