using System.Text;

namespace dotnetConf2023.Shared.Abstraction.Queries;

public abstract class BasePagination
{
    private readonly string _orderBy;
    public int Page { get; }
    public int Size { get; }
    public int CalculateSkip() => (Page - 1) * Size;
    public string OrderBy => ValidateOrderBy(_orderBy);
    protected Dictionary<string, string?>? ValidOrderByColumnsDictionary { get; init; }
    protected string? DefaultOrderBy { get; init; }

    protected BasePagination(int page, int size, string orderBy)
    {
        _orderBy = orderBy;
        Page = page;
        Size = size;

        if (Size > 100)
            Size = 100;

        if (Size < 1)
            Size = 10;

        if (Page < 1)
            Page = 1;
    }

    private string ValidateOrderBy(string orderBy)
    {
        if (string.IsNullOrWhiteSpace(orderBy) || ValidOrderByColumnsDictionary is null ||
            !ValidOrderByColumnsDictionary.Any())
        {
            return DefaultOrderBy ?? string.Empty;
        }

        var orderByParts = orderBy.Split(',', StringSplitOptions.RemoveEmptyEntries);

        var orderByStringBuilder = new StringBuilder();

        foreach (var orderByPart in orderByParts)
        {
            var columnName = orderByPart.Trim(' ').Split(' ')[0].ToLower();

            if (!ValidOrderByColumnsDictionary.TryGetValue(columnName, out var orderByColumn))
                continue;

            var orderByDirection = orderByPart.EndsWith(" desc", StringComparison.OrdinalIgnoreCase)
                ? " desc"
                : string.Empty;
            var orderByValue = $"{orderByColumn}{orderByDirection}, ";
            orderByStringBuilder.Append(orderByValue);
        }

        var validatedOrderBy = orderByStringBuilder.ToString().TrimEnd(',', ' ');

        return string.IsNullOrEmpty(validatedOrderBy) ? DefaultOrderBy ?? string.Empty : validatedOrderBy;
    }
}