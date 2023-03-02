namespace dotnetConf2023.Core.Identity.Commands.ChangePassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(e => e.CurrentPassword).NotEmpty().MaximumLength(256);
        RuleFor(e => e.NewPassword).NotEmpty().MaximumLength(256);
    }
}