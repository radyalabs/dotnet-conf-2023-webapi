namespace dotnetConf2023.Shared.Abstraction.Time;

public interface IClock
{
    DateTime CurrentDate();
    DateTime CurrentServerDate();
}