namespace dotnetConf2023.Core.UserManagement.Commands.CreateUser;

public sealed record CreateUserCommand : ICommand<Result>
{
    public string Username { get; set; } = null!;
    public string NormalizedUsername => Username.ToUpperInvariant();
    public string Fullname { get; set; } = null!;
    public string Password { get; set; } = null!;
}