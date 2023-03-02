using dotnetConf2023.Core.Validators;

namespace dotnetConf2023.Core.UserManagement.Commands.ChangePasswordUser;

public class ChangePasswordUserCommandValidator : AbstractValidator<ChangePasswordUserCommand>
{
    public ChangePasswordUserCommandValidator()
    {
        RuleFor(e => e.NewPassword).SetValidator(new PasswordValidator());
    }
}