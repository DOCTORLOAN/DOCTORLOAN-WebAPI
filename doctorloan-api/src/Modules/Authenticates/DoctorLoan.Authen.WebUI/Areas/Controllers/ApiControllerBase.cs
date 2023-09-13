using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace DoctorLoan.Authen.WebUI.Areas.Controllers;

[ApiController]
[Route("api/auth-module/[controller]")]
[Authorize]
[DoctorLoan.Application.Common.Security.RequireReauthenticationAttribute]
public abstract class ApiControllerBase : ControllerBase
{
    private readonly ISender _mediator = null!;
    protected ISender Mediator => _mediator ?? HttpContext.RequestServices.GetRequiredService<ISender>();
}


