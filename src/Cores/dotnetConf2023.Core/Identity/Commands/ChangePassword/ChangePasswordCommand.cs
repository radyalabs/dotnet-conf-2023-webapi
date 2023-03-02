namespace dotnetConf2023.Core.Identity.Commands.ChangePassword;

public sealed record ChangePasswordCommand : ICommand<Result>
{
    private Guid _userId;

    public void SetUserId(Guid userId)
    {
        _userId = userId;
    }

    public Guid GetUserId() => _userId;
    public string CurrentPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
    public bool ForceReLogin { get; set; }
}