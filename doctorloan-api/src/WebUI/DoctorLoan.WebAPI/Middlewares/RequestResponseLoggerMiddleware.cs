using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Models.Settings;
using Microsoft.Extensions.Options;

namespace DoctorLoan.WebAPI.Middlewares;

public class RequestResponseLoggerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly bool _isRequestResponseLoggingEnabled;
    protected readonly ILogger<RequestResponseLoggerMiddleware> _logger;
    private readonly ICurrentUserService _currentUserService;
    private readonly SerilogConfiguration _serilogConfiguration;

    public RequestResponseLoggerMiddleware(RequestDelegate next, IConfiguration config,
        ICurrentUserService currentUserService,
        IOptions<SerilogConfiguration> serilogConfiguration,
        ILogger<RequestResponseLoggerMiddleware> logger)
    {
        _serilogConfiguration = serilogConfiguration.Value;
        _next = next;
        _isRequestResponseLoggingEnabled = _serilogConfiguration.EnableRequestResponseLogging;
        _logger = logger;
        _currentUserService = currentUserService;
    }

    private class RequestResponseLog
    {
        public string Request { get; set; }
        public DateTimeOffset RequestTime { get; set; }
        public string Response { get; set; }
        public DateTimeOffset ResponseTime { get; set; }
    }

    private async Task<string> FormatResponse(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        string responseBody = await new StreamReader(response.Body).ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);

        var userInfo = _currentUserService.UserId.ToString();

        var logMessage = $"[Response]:\n" +
                $"\tUser: {userInfo}, \n" +
                $"\tStatusCode: {response.StatusCode}\n" +
                $"\tContentType: {response.ContentType}\n" +
                $"\tHeaders: {FormatHeaders(response.Headers)}\n" +
                $"\tBody: {responseBody}";

        return logMessage;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        // Middleware is enabled only when the EnableRequestResponseLogging config value is set.
        if (_isRequestResponseLoggingEnabled)
        {
            // Temporarily replace the HttpResponseStream, which is a write-only stream, with a MemoryStream to capture it's value in-flight.
            var originalResponseBody = httpContext.Response.Body;
            using var newResponseBody = new MemoryStream();
            httpContext.Response.Body = newResponseBody;

            // Call the next middleware in the pipeline
            await _next(httpContext);

            var responseBodyText = await FormatResponse(httpContext.Response);
            var response = new RequestResponseLog
            {
                ResponseTime = DateTimeOffset.UtcNow,
                Response = responseBodyText
            };
            _logger.LogInformation(response.Response);

            await newResponseBody.CopyToAsync(originalResponseBody);
        }
        else
        {
            await _next(httpContext);
        }
    }

    private static string FormatHeaders(IHeaderDictionary headers) => string.Join(", ", headers.Select(kvp => $"{{{kvp.Key}: {string.Join(", ", kvp.Value)}}}"));

}

