using dotnetConf2023.Shared.Abstraction.Time;
using Moq;

namespace dotnetConf2023.UnitTests.Extensions;

public static class ClockBuilderExtensions
{
    public static Mock<IClock> Create()
    {
        var mock = new Mock<IClock>();

        mock.Setup(e => e.CurrentDate()).Returns(DateTime.Now);
        mock.Setup(e => e.CurrentServerDate()).Returns(DateTime.Now);

        return mock;
    }
}