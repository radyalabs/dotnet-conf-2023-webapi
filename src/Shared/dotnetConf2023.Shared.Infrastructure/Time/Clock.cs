using dotnetConf2023.Shared.Abstraction.Time;

namespace dotnetConf2023.Shared.Infrastructure.Time;

internal sealed class Clock : IClock
{
    private readonly ClockOptions _options;

    public Clock(ClockOptions options)
    {
        _options = options;
    }

    public DateTime CurrentDate() => DateTime.UtcNow;

    public DateTime CurrentServerDate() => CurrentDate().AddHours(_options.Hours);
}