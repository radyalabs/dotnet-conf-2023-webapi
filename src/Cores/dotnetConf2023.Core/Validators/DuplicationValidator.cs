namespace dotnetConf2023.Core.Validators;

public class DuplicationValidator : AbstractValidator<List<string>>
{
    public DuplicationValidator(bool isRequired = true)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        if (isRequired)
            RuleFor(x => x).Custom((x, ctx) =>
            {
                var nextList = x.Distinct().ToList();

                if (nextList.Count != x.Count)
                    ctx.AddFailure("List", "Duplication record");
            });
        else
            RuleFor(x => x).NotNull().Custom((x, ctx) =>
            {
                var nextList = x.Distinct().ToList();

                if (nextList.Count != x.Count)
                    ctx.AddFailure("List", "Duplication record");
            });
    }
}