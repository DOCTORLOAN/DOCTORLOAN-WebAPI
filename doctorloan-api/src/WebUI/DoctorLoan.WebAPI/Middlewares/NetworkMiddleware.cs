using System.Net;
using System.Net.NetworkInformation;
using DoctorLoan.Application.Models.Commons;

namespace DoctorLoan.WebAPI.Middlewares;

public class NetworkMiddleware
{
    private readonly RequestDelegate _next;
    public NetworkMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext httpContext)
    {
        using Ping ping = new();
        string hostName = "stackoverflow.com";
        try
        {
            PingReply reply = await ping.SendPingAsync(hostName);
            var success = reply is { Status: IPStatus.Success };
            if (!success)
            {
                await NetworkWrong(httpContext);
            }

            await _next(httpContext);
        }
        catch
        {
            await NetworkWrong(httpContext);
        }
    }

    private async Task NetworkWrong(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = (int)HttpStatusCode.NetworkAuthenticationRequired;
        await httpContext.Response.WriteAsJsonAsync(Result.Failed(ServiceError.WithCustomMessage("Network not available!!!")));
    }

}
