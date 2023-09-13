
using DoctorLoan.Application.Common.Exceptions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Domain.Const.Validations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DoctorLoan.WebAPI.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;
    private readonly bool isDevelopment = false;
    private readonly ICurrentTranslateService _currentTranslateService;

    public ApiExceptionFilterAttribute(IWebHostEnvironment app, ICurrentTranslateService currentTranslateService)
    {
        // Register known exception types and handlers.
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(ValidationException), HandleValidationException },
                { typeof(NotFoundException), HandleNotFoundException },
                { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
                { typeof(UnauthorizedResult), HandleUnauthorizedAccessException },
                { typeof(ForbiddenAccessException), HandleForbiddenAccessException },
                { typeof(ForbidResult), HandleForbiddenAccessException },
            };
        isDevelopment = app.EnvironmentName == "Development";
        _currentTranslateService = currentTranslateService;
    }

    public override void OnException(ExceptionContext context)
    {
        HandleException(context);

        base.OnException(context);
    }

    private void HandleException(ExceptionContext context)
    {
        Type type = context.Exception.GetType();
        if (_exceptionHandlers.ContainsKey(type))
        {
            _exceptionHandlers[type].Invoke(context);
            return;
        }

        if (!context.ModelState.IsValid)
        {
            HandleInvalidModelStateException(context);
            return;
        }
        HandleServerError(context);

    }

    private void HandleValidationException(ExceptionContext context)
    {
        var exception = (ValidationException)context.Exception;

        var details = new ValidationError(exception.Errors);

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }

    private void HandleInvalidModelStateException(ExceptionContext context)
    {
        var details = new ValidationProblemDetails(context.ModelState)
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }

    private void HandleNotFoundException(ExceptionContext context)
    {
        var exception = (NotFoundException)context.Exception;

        var details = new ProblemDetails()
        {
            Title = _currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.NotFound),
            Detail = exception.Message
        };

        context.Result = new NotFoundObjectResult(details);

        context.ExceptionHandled = true;
    }

    private void HandleUnauthorizedAccessException(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = _currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.ForbiddenError),
            Type = "https://tools.ietf.org/html/rfc7235#section-3.1"
        };

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status401Unauthorized
        };

        context.ExceptionHandled = true;
    }

    private void HandleForbiddenAccessException(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status403Forbidden,
            Title = _currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.Forbidden),
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3"

        };

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status403Forbidden,

        };

        context.ExceptionHandled = true;
    }

    private void HandleServerError(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = _currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.InternalServerError),
            Detail = isDevelopment ? context.Exception.ToString() : null
        };

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        context.ExceptionHandled = true;
    }
}


public class ValidationError
{
    public string Error { get; set; }

    public IDictionary<string, string[]> Errors { get; }

    public ValidationError(IDictionary<string, string[]> errors)
    {
        Errors = errors;
        Error = string.Join(", ", errors.Select(s => string.Join(", ", s.Value)));
    }
}
