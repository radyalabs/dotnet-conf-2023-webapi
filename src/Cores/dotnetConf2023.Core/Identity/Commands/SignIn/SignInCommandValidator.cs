namespace dotnetConf2023.Core.Identity.Commands.SignIn;

public sealed class SignInCommandValidator : AbstractValidator<SignInCommand>
{
    public SignInCommandValidator()
    {
        RuleFor(e => e.ClientId).NotEmpty().MaximumLength(256);
        RuleFor(e => e.Username).NotEmpty().MaximumLength(256);
        RuleFor(e => e.Password).NotEmpty().MaximumLength(256);
    }
}