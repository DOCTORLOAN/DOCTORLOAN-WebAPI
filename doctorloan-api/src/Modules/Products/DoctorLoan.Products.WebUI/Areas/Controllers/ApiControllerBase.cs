using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace DoctorLoan_Products.WebUI.Areas.Controllers;

[ApiController]
[Route("api/product-module/[controller]/[action]")]
[Authorize]
[DoctorLoan.Application.Common.Security.RequireReauthenticationAttribute]
public abstract class ApiControllerBase : ControllerBase
{
    private readonly ISender _mediator = null!;
    protected ISender Mediator => _mediator ?? HttpContext.RequestServices.GetRequiredService<ISender>();
}
