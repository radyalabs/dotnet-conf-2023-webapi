using dotnetConf2023.Core.Identity.Commands.ChangeEmail;
using dotnetConf2023.Core.Identity.Commands.ChangePassword;
using dotnetConf2023.Core.Identity.Commands.RefreshToken;
using dotnetConf2023.Core.Identity.Commands.SignIn;
using dotnetConf2023.Core.Identity.Commands.VerifyEmail;
using dotnetConf2023.Core.Identity.Queries.GetMe;
using Microsoft.AspNetCore.Authorization;

namespace dotnetConf2023.Api.Controllers;

public sealed class IdentityController : BaseController
{
    /// <summary>
    /// Sign In API.
    /// This API generally requires username, and password.
    /// Client Id is used for tracking user behavior and its rate limiting,
    /// also device type is used for identify user log in, using which User Agent.
    /// </summary>
    /// <param name="request">See <see cref="SignInCommand"/></param>
    /// <param name="cancellationToken">CancellationToken see <see cref="CancellationToken"/></param>
    /// <returns>Produces <see cref="dotnetConf2023.Shared.Abstraction.Auth.JsonWebToken"/></returns>
    [HttpPost("sign-in")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignInAsync([FromBody] SignInCommand request, CancellationToken cancellationToken)
        => ConstructResult(await Sender.Send(request, cancellationToken));

    /// <summary>
    /// Get Me API.
    /// This API produces information for the user
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>Produces <see cref="dotnetConf2023.Core.Responses.MeResponse"/></returns>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetMeAsync(CancellationToken cancellationToken)
        => ConstructResult(await Sender.Send(new GetMeQuery { UserId = Context!.Identity.Id }, cancellationToken));

    /// <summary>
    /// Change Password User API
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("password")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordCommand command,
        CancellationToken cancellationToken)
    {
        command.SetUserId(Context!.Identity.Id);
        return ConstructResult(await Sender.Send(command, cancellationToken));
    }

    /// <summary>
    /// Refresh Token API
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("refresh-token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenCommand command,
        CancellationToken cancellationToken)
        => ConstructResult(await Sender.Send(command, cancellationToken));

    /// <summary>
    /// Change Email API
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("email")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangeEmailAsync([FromBody] ChangeEmailCommand command,
        CancellationToken cancellationToken)
    {
        command.SetUserId(Context!.Identity.Id);
        return ConstructResult(await Sender.Send(command, cancellationToken));
    }

    /// <summary>
    /// Verify Email API
    /// </summary>
    /// <param name="code"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("verify-email")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> VerifyEmailAsync([FromQuery] string code,
        CancellationToken cancellationToken)
    {
        var command = new VerifyEmailCommand { Code = code };
        return ConstructResult(await Sender.Send(command, cancellationToken));
    }
}