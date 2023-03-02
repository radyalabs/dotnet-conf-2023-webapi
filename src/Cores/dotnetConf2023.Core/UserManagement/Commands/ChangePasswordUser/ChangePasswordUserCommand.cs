namespace dotnetConf2023.Core.UserManagement.Commands.ChangePasswordUser;

public sealed record ChangePasswordUserCommand : ICommand<Result>
{
    public Guid UserId { get; set; }
    public string NewPassword { get; set; } = null!;
}