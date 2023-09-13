namespace DoctorLoan.WebAPI.Middlewares;


public static class CustomMiddlewareExtensions
{
    public static void UseRequestResponseLogging(this IApplicationBuilder app) => app.UseMiddleware<RequestResponseLoggerMiddleware>();
    //public static void UseNetworkAvaliable(this IApplicationBuilder app) => app.UseMiddleware<NetworkMiddleware>();
}