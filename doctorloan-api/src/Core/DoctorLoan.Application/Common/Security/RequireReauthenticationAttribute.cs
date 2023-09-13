using System.IdentityModel.Tokens.Jwt;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Domain.Const.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace DoctorLoan.Application.Common.Security;
public class RequireReauthenticationAttribute : Attribute, IAsyncResourceFilter
{
    public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
    {
        var currentUserService = context.HttpContext.RequestServices.GetService<ICurrentUserService>();
        var hasAllowAnonymous = context.ActionDescriptor.EndpointMetadata.Any(s => s.GetType() == typeof(AllowAnonymousAttribute));

        var notAuthorized = string.IsNullOrEmpty(context.HttpContext.Request.Headers.Authorization);

        if (notAuthorized || currentUserService?.CurrentUser?.ValidUnixTime == null || hasAllowAnonymous) await next();

        var currentTranslateService = context.HttpContext.RequestServices.GetService<ICurrentTranslateService>();
        var idToken = context.HttpContext.Request.Headers["Id-Token"].ToString();

        if (string.IsNullOrEmpty(idToken))
        {
            context.Result = new JsonResult(new { Error = "Headers Missing Id-Token" }) { StatusCode = 400 };
        }
        else
        {
            try
            {
                var token = new JwtSecurityToken(jwtEncodedString: idToken);
                var foundAuthTime = int.TryParse(token.Claims.First(c => c.Type == "auth_time")?.Value, out int authTime);

                var validTime = authTime - currentUserService.CurrentUser.ValidUnixTime >= 0;
                if (foundAuthTime && validTime)
                {
                    await next();
                }
                else
                {
                    context.Result = new JsonResult(new ServiceError(currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.TokenExpired), 001)) { StatusCode = StatusCodes.Status401Unauthorized };
                }
            }
            catch (Exception)
            {
                context.Result = new JsonResult(new { Error = "Invalid Id-Token" }) { StatusCode = 400 };
            }
        }
    }
}
