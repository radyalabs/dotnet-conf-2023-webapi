using FluentValidation;

namespace dotnetConf2023.Shared.Infrastructure.Validators;

public sealed class IndonesianPhoneNumberValidator : AbstractValidator<string>
{
    public IndonesianPhoneNumberValidator()
    {
        var phoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();

        RuleFor(e => e).Custom((e, ctx) =>
        {
            try
            {
                var x = phoneNumberUtil.Parse(e, "ID");
                if (x != null && x.CountryCode != 62)
                    throw new Exception();
            }
            catch
            {
                ctx.AddFailure(propertyName: "s", errorMessage: "Invalid phone number format");
            }
        });
    }
}