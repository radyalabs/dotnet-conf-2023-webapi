using System.Text.RegularExpressions;
using FluentValidation;

namespace dotnetConf2023.Shared.Infrastructure.Validators;

public class StringIsBase64Validator : AbstractValidator<string>
{
    public StringIsBase64Validator()
    {
        RuleFor(e => e).Custom((s, ctx) =>
        {
            s = s.Trim();
            var x = (s.Length % 4 == 0) && Regex.IsMatch(s, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
            if (!x)
                ctx.AddFailure("Invalid base 64 format");
        });
    }
}