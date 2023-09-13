using System.Net;
using DoctorLoan.Application.Common.Security;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Domain.Enums.Authorizations;
using DoctorLoan.Order.Application.Features.Commands;
using DoctorLoan.Order.Application.Features.Dtos;
using DoctorLoan.Order.Application.Features.Queries;
using Microsoft.AspNetCore.Mvc;

namespace DoctorLoan.Order.WebUI.Areas;
public class OrderController : ApiControllerBase
{
    #region Filter
    [HttpGet]
    [Route("filter")]
    [Authorize(PermissionModuleEnum.Order, PermissionActionEnum.Read)]
    [ProducesResponseType(typeof(Result<PaginatedList<OrderDto>>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> FilterOrder([FromQuery] FilterOrderQuery query, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(query, cancellationToken));
    }

    [HttpGet]
    [Route("{id:int}")]
    [ProducesResponseType(typeof(Result<OrderDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetOrderById([FromRoute] int id, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(new GetOrderByIdQuery(id), cancellationToken));
    }
    #endregion

    #region CRUD customer

    [Route("create")]
    [ProducesResponseType(typeof(Result<int>), (int)HttpStatusCode.OK)]
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] AddOrderCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [Route("update-status")]
    [Authorize(PermissionModuleEnum.Order, PermissionActionEnum.Update)]
    [ProducesResponseType(typeof(Result<bool>), (int)HttpStatusCode.OK)]
    [HttpPatch]
    public async Task<IActionResult> UpdateStatus([FromBody] UpdateStatusCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }
    #endregion
}