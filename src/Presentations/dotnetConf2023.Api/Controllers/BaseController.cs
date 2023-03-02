using dotnetConf2023.Shared.Abstraction.Contexts;
using dotnetConf2023.Shared.Abstraction.Queries;

namespace dotnetConf2023.Api.Controllers;

/// <summary>
/// Represents the base API controller.
/// </summary>
[Route("api/[controller]")]
public class BaseController : ControllerBase
{
    protected ISender Sender => HttpContext.RequestServices.GetRequiredService<ISender>();
    private IContext? _context;
    protected IContext? Context => _context ??= HttpContext.RequestServices.GetService<IContext>();

    /// <summary>
    /// Creates an <see cref="OkObjectResult"/> that produces a <see cref="StatusCodes.Status200OK"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The created <see cref="OkObjectResult"/> for the response.</returns>
    protected new IActionResult Ok(object value) => base.Ok(value);

    /// <summary>
    /// Creates an <see cref="NotFoundResult"/> that produces a <see cref="StatusCodes.Status404NotFound"/>.
    /// </summary>
    /// <returns>The created <see cref="NotFoundResult"/> for the response.</returns>
    protected new IActionResult NotFound() => NotFound("The requested resource was not found.");

    protected IActionResult ConstructResult<TValue>(Result<TValue> value)
        => value.IsSuccess ? base.Ok(value.Value) : ErrorResult(value.Error);

    protected IActionResult ConstructResult(Result value)
        => value.IsSuccess ? base.NoContent() : ErrorResult(value.Error);

    protected IActionResult ConstructResult<TValue>(PagedList<TValue> value)
        => Ok(value);

    private IActionResult ErrorResult(Error error)
    {
        var err = error.StatusCode;

        return err switch
        {
            400 => base.BadRequest(error),
            403 => Forbid(),
            404 => NotFound(),
            _ => StatusCode(500, error)
        };
    }
}