namespace dotnetConf2023.Core.UserManagement.Commands.EditUser;

public class EditUserCommandValidator : AbstractValidator<EditUserCommand>
{
    public EditUserCommandValidator()
    {
        RuleFor(e => e.FullName)
            .MinimumLength(4)
            .MaximumLength(100)
            .When(e => !string.IsNullOrWhiteSpace(e.FullName));

        RuleFor(e => e.AboutMe)
            .MaximumLength(500)
            .When(e => !string.IsNullOrWhiteSpace(e.AboutMe));
    }
}