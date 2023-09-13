using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace DoctorLoan.Customer.WebUI.Areas.Controllers;


[ApiController]
[Route("api/customer-module/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    private readonly ISender _mediator = null!;
    protected ISender Mediator => _mediator ?? HttpContext.RequestServices.GetRequiredService<ISender>();
}