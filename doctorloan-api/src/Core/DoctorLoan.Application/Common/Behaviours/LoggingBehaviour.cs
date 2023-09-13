using DoctorLoan.Application.Interfaces.Commons;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger _logger;
    private readonly ICurrentUserService _currentUserService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LoggingBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _currentUserService = currentUserService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = _currentUserService.UserId;

        _logger.LogInformation("DoctorLoan.Users Request: {Name} {@UserId} {@Request} {@Header}",
            requestName, userId, request, FormatHeaders(_httpContextAccessor.HttpContext.Request.Headers));
        await Task.FromResult(0);
    }

    private static string FormatHeaders(IHeaderDictionary headers) => string.Join(", ", headers.Select(kvp => $"{{{kvp.Key}: {string.Join(", ", kvp.Value)}}}"));
}
