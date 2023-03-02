namespace dotnetConf2023.Core.Identity.Commands.SignIn;

public sealed record SignInCommand : ICommand<Result<JsonWebToken>>
{
    public SignInCommand()
    {
        _deviceType = DeviceType.Others;
    }

    private DeviceType _deviceType;

    /// <summary>
    /// Other word of device id 
    /// </summary>
    public string ClientId { get; set; } = null!;

    /// <summary>
    /// Username, usually email or anything else
    /// </summary>
    public string Username { get; set; } = null!;

    /// <summary>
    /// User password
    /// </summary>
    public string Password { get; set; } = null!;

    public void SetDeviceType(DeviceType deviceType)
    {
        _deviceType = deviceType;
    }

    public DeviceType GetDeviceType() => _deviceType;
}