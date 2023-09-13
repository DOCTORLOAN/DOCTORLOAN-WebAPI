using System.Net;
using DoctorLoan.Application.Common.Security;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Common.Application.Features.Menus.Admin.Queries;
using DoctorLoan.Common.Application.Features.Queries.GetRolesQuery;
using DoctorLoan.Domain.Enums.Authorizations;
using DoctorLoan.Domain.Enums.Commons;
using Microsoft.AspNetCore.Mvc;

namespace DoctorLoan.Common.WebUI.Areas.Controllers;

public class CommonController : ApiControllerBase
{

    [Route("option/{type}")]
    [HttpGet]
    [Authorize(PermissionModuleEnum.User, PermissionActionEnum.Read)]
    [ProducesResponseType(typeof(Result<List<Option>>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetOptions(OptionType type, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(new GetOptionsQuery(type), cancellationToken));
    }
    [HttpGet]

    [Route("getmenus")]
    public async Task<IActionResult> GetMenus(CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(new GetMenuQuery(), cancellationToken));
    }

}
