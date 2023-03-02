using dotnetConf2023.Shared.Abstraction.Primitives;

namespace dotnetConf2023.Shared.Infrastructure.Api;

/// <summary>
/// Represents API an error response.
/// </summary>
public class ApiErrorResponse
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApiErrorResponse"/> class.
    /// </summary>
    /// <param name="innerErrors">The errors.</param>
    public ApiErrorResponse(IReadOnlyCollection<Error> innerErrors)
    {
        InnerErrors = innerErrors;
        Error = InnerErrors.First();
        if (InnerErrors.Count == 1)
            InnerErrors = null!;
    }

    /// <summary>
    /// Gets the errors.
    /// </summary>
    public IReadOnlyCollection<Error> InnerErrors { get; }

    /// <summary>
    /// Get summary error
    /// </summary>
    public Error Error { get; }
}