using System.Net;
using DoctorLoan.Application.Models.Authenticates;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Authen.Application.Features.Authenticates;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorLoan.Authen.WebUI.Areas.Controllers;
public class AuthController : ApiControllerBase
{
    //[Authorize(Policy = "TradieOnly")]
    [AllowAnonymous]
    [HttpPost]
    [Route("signin")]
    [ProducesResponseType(typeof(Result<JWTTokens>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> SignIn([FromBody] SignInCommand request, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(request, cancellationToken));
    }


    [HttpPost]
    [Route("refresh")]
    [ProducesResponseType(typeof(Result<JWTTokens>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        request.AccessToken = await HttpContext.GetTokenAsync("access_token");
        return Ok(await Mediator.Send(request, cancellationToken));
    }
}
