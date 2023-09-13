using System.Net;
using DoctorLoan.Application.Common.Security;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Domain.Enums.Authorizations;
using DoctorLoan.User.Application.Features.Users;
using DoctorLoan.User.Application.Features.Users.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace DoctorLoan.User.WebUI.Areas.Controllers;

public class UserController : ApiControllerBase
{
    #region Filter User

    [Route("me")]
    [HttpGet]
    [ProducesResponseType(typeof(Result<UserDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Me(CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(new GetMeQuery(), cancellationToken));
    }

    [HttpGet]
    [Route("filter")]
    [Authorize(PermissionModuleEnum.User, PermissionActionEnum.Read)]
    [ProducesResponseType(typeof(Result<PaginatedList<UserDto>>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> FilterUser([FromQuery] FilterUserQuery query, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(query, cancellationToken));
    }


    [Route("{id:int}")]
    [HttpGet]
    [Authorize(PermissionModuleEnum.User, PermissionActionEnum.Read)]
    [ProducesResponseType(typeof(Result<UserDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetUserById([FromRoute] int id, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(new GetUserByIdQuery(id), cancellationToken));
    }

    #endregion

    #region CRUD User

    [Route("create")]
    [ProducesResponseType(typeof(Result<int>), (int)HttpStatusCode.OK)]
    [Authorize(PermissionModuleEnum.User, PermissionActionEnum.Create)]
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] AddUserCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [Route("update")]
    [ProducesResponseType(typeof(Result<int>), (int)HttpStatusCode.OK)]
    [Authorize(PermissionModuleEnum.User, PermissionActionEnum.Update)]
    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(PermissionModuleEnum.User, PermissionActionEnum.Delete)]
    [ProducesResponseType(typeof(Result<bool>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteUser([FromRoute] int id, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(new DeleteUserCommand(id), cancellationToken));
    }

    [Microsoft.AspNetCore.Authorization.AllowAnonymous]
    [HttpPatch]
    [Route("reset-password")]
    [ProducesResponseType(typeof(Result<bool>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> ResetPassword([FromBody] ForgetPasswordCommand request, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(request, cancellationToken));
    }

    #endregion
}