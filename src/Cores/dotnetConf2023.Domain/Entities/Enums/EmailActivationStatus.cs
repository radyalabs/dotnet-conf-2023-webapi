namespace dotnetConf2023.Domain.Entities.Enums;

public enum EmailActivationStatus
{
    /// <summary>
    /// Process email activation skipped.
    /// </summary>
    Skip,

    /// <summary>
    /// Process needs email activation to proceed.
    /// </summary>
    NeedActivation,

    /// <summary>
    /// Email activated.
    /// </summary>
    Activated
}