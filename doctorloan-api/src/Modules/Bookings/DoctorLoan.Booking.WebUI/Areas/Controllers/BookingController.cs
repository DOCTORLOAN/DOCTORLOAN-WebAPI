using System.Net;
using DoctorLoan.Application.Common.Security;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Booking.Application.Features.Commands;
using DoctorLoan.Booking.Application.Features.Dtos;
using DoctorLoan.Booking.Application.Features.Queries;
using DoctorLoan.Domain.Enums.Authorizations;
using Microsoft.AspNetCore.Mvc;

namespace DoctorLoan.Booking.WebUI.Areas.Controllers;
public class BookingController : ApiControllerBase
{
    #region Filter
    [HttpGet]
    [Route("filter")]
    [ProducesResponseType(typeof(Result<PaginatedList<BookingDto>>), (int)HttpStatusCode.OK)]
    [Authorize(PermissionModuleEnum.Book, PermissionActionEnum.Read)]
    public async Task<IActionResult> FilterBooking([FromQuery] FilterBookingQuery query, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(query, cancellationToken));
    }

    [HttpGet]
    [Route("{id:int}")]
    [ProducesResponseType(typeof(Result<BookingDto>), (int)HttpStatusCode.OK)]
    [Authorize(PermissionModuleEnum.Book, PermissionActionEnum.Read)]
    public async Task<IActionResult> GetBookingById([FromRoute] int id, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(new GetBookingByIdQuery(id), cancellationToken));
    }

    #endregion

    #region CRUD customer

    [Route("create")]
    [ProducesResponseType(typeof(Result<int>), (int)HttpStatusCode.OK)]
    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] AddBookingCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [Route("update-status")]
    [ProducesResponseType(typeof(Result<int>), (int)HttpStatusCode.OK)]
    [Authorize(PermissionModuleEnum.Book, PermissionActionEnum.Update)]
    [HttpPatch]
    public async Task<IActionResult> UpdateStatusBooking([FromBody] UpdateBookingCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }
    #endregion
}
