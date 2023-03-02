namespace dotnetConf2023.Domain.Entities.Enums;

public enum UserState
{
    /// <summary>
    /// State that user is no longer active, can not access the application
    /// </summary>
    InActive = 0,

    /// <summary>
    /// State that user is active, can access the application
    /// </summary>
    Active = 1,

    /// <summary>
    /// State that user is temporary locked, can not access the application
    /// </summary>
    Locked = 2
}