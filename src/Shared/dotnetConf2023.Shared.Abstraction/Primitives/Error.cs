namespace dotnetConf2023.Shared.Abstraction.Primitives;

/// <summary>
/// Represents a concrete domain error.
/// </summary>
public sealed class Error : ValueObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="message">The error message.</param>
    public Error(string code, string message)
    {
        Code = code;
        Message = message;
        StatusCode = 400;
    }

    internal Error(int statusCode, string code, string message) : this(code, message)
    {
        StatusCode = statusCode;
    }

    public int StatusCode { get; }

    /// <summary>
    /// Gets the error code.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Gets the empty error instance.
    /// </summary>
    internal static Error None => new(string.Empty, string.Empty);

    /// <summary>
    /// Create error instance.
    /// </summary>
    /// <param name="message">string</param>
    /// <returns>See <see cref="Error"/></returns>
    public static Error Create(string message) => new(string.Empty, message);

    /// <summary>
    /// Create error instance.
    /// </summary>
    /// <param name="code">string</param>
    /// <param name="message">string</param>
    /// <returns>See <see cref="Error"/></returns>
    public static Error Create(string code, string message) => new(code, message);

    /// <summary>
    /// Create error instance.
    /// </summary>
    /// <param name="statusCode">int</param>
    /// <param name="code">string</param>
    /// <param name="message">string</param>
    /// <returns>See <see cref="Error"/></returns>
    public static Error Create(int statusCode, string code, string message) => new(statusCode, code, message);

    public static implicit operator string(Error? error) => error?.Code ?? string.Empty;

    /// <inheritdoc />
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Code;
        yield return Message;
    }
}