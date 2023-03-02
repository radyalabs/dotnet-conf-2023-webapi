using dotnetConf2023.Shared.Abstraction.Time;

namespace dotnetConf2023.IntegrationTests.Dependencies;

internal class Clock : IClock
{
    public DateTime CurrentDate() => DateTime.UtcNow;

    public DateTime CurrentServerDate() => CurrentDate();
}