namespace dotnetConf2023.Core.Identity.Commands.RefreshToken;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(e => e.ClientId).NotEmpty();
        RuleFor(e => e.RefreshToken).NotEmpty();
    }
}