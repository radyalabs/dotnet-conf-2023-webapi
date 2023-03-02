using dotnetConf2023.Shared.Abstraction.Storage;
using Moq;

namespace dotnetConf2023.UnitTests.Extensions;

public static class RequestStorageBuilderExtensions
{
    public static Mock<IRequestStorage> Create()
    {
        var mock = new Mock<IRequestStorage>();
        return mock;
    }
}