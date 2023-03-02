namespace dotnetConf2023.Core.Validators;

public class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(e => e).NotEmpty().MinimumLength(8).MaximumLength(64);
    }
}