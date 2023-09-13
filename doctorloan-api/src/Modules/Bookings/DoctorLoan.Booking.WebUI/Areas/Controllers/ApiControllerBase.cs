using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace DoctorLoan.Booking.WebUI.Areas.Controllers;

[ApiController]
[Route("api/booking-module/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    private readonly ISender _mediator = null!;
    protected ISender Mediator => _mediator ?? HttpContext.RequestServices.GetRequiredService<ISender>();
}
