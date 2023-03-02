using dotnetConf2023.Core.Responses;
using dotnetConf2023.Core.UserManagement.Commands.ChangePasswordUser;
using dotnetConf2023.Core.UserManagement.Commands.CreateUser;
using dotnetConf2023.Core.UserManagement.Commands.EditUser;
using dotnetConf2023.Core.UserManagement.Queries.GetUserById;
using dotnetConf2023.Core.UserManagement.Queries.GetUsers;
using dotnetConf2023.Domain;
using dotnetConf2023.Shared.Abstraction.Models;
using dotnetConf2023.Shared.Abstraction.Queries;
using Microsoft.AspNetCore.Authorization;

namespace dotnetConf2023.Api.Controllers;

[Authorize(Roles = RoleConstant.Administrator)]
public sealed class UserManagementController : BaseController
{
    /// <summary>
    /// Get Roles API
    /// </summary>
    /// <returns></returns>
    [HttpGet("roles")]
    [ProducesResponseType(typeof(List<DropdownKeyValue>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public IActionResult GetRoles()
        => Ok(RoleConstant.Dictionary
            .Select(e => new DropdownKeyValue(e.Key, e.Value)).ToList());

    /// <summary>
    /// Create User API
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("users")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserCommand command,
        CancellationToken cancellationToken)
        => ConstructResult(await Sender.Send(command, cancellationToken));

    /// <summary>
    /// Edit User API
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("users")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EditUserAsync([FromBody] EditUserCommand command,
        CancellationToken cancellationToken)
        => ConstructResult(await Sender.Send(command, cancellationToken));

    /// <summary>
    /// Change Password User API
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("users/password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangePasswordUserAsync([FromBody] ChangePasswordUserCommand command,
        CancellationToken cancellationToken)
        => ConstructResult(await Sender.Send(command, cancellationToken));

    /// <summary>
    /// Get Users API
    /// </summary>
    /// <param name="username"></param>
    /// <param name="fullName"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="orderBy"></param>
    /// <returns></returns>
    [HttpGet("users")]
    [ProducesResponseType(typeof(PagedList<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUsersAsync(
        string? username,
        string? fullName,
        int page,
        int pageSize,
        string orderBy)
        => ConstructResult(await Sender.Send(new GetUsersQuery(page, pageSize, orderBy, fullName, username)));

    /// <summary>
    /// Get User by Id API
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("users/{id:Guid}")]
    [ProducesResponseType(typeof(PagedList<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserByIdAsync(
        [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);
        return ConstructResult(await Sender.Send(query, cancellationToken));
    }
}