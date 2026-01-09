namespace DoctorLoan.Domain.Common.Errors;

/// <summary>
/// Represents a business service error.
/// </summary>
public class ServiceError
{
    public static readonly ServiceError DefaultError =
        new("An unexpected error has occurred.");

    public string Code { get; }

    public string Message { get; }

    public ServiceError(string message, string? code = null)
    {
        Message = message;
        Code = code ?? "ERROR";
    }

    public static ServiceError WithCustomMessage(string message)
    {
        return new ServiceError(message);
    }

    public static ServiceError WithCode(string code, string message)
    {
        return new ServiceError(message, code);
    }
}
