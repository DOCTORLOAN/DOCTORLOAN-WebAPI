using DoctorLoan.Application.Common.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;


namespace DoctorLoan.News.WebUI.Areas.Controllers;

[ApiController]
[Route("api/news-module/[controller]/[action]")]
[Microsoft.AspNetCore.Authorization.Authorize]
[RequireReauthentication]
public abstract class ApiControllerBase : ControllerBase
{
    private readonly ISender _mediator = null!;
    protected ISender Mediator => _mediator ?? HttpContext.RequestServices.GetRequiredService<ISender>();
}
