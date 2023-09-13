using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace DoctorLoan.User.WebUI.Areas.Controllers;

[ApiController]
[Route("api/user-module/[controller]")]
[Authorize]
public abstract class ApiControllerBase : ControllerBase
{
    private readonly ISender _mediator = null!;
    protected ISender Mediator => _mediator ?? HttpContext.RequestServices.GetRequiredService<ISender>();
}