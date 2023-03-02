namespace dotnetConf2023.Core.Identity.Commands.VerifyEmail;

public sealed record VerifyEmailCommand : ICommand<Result>
{
    public string Code { get; set; } = null!;
}