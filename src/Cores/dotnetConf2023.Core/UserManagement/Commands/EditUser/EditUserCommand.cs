namespace dotnetConf2023.Core.UserManagement.Commands.EditUser;

public sealed record EditUserCommand : ICommand<Result>
{
    public Guid UserId { get; init; }
    public string? FullName { get; init; }
    public string? AboutMe { get; init; }
}