using Newtonsoft.Json;
using DoctorLoan.Domain.Common.Errors;

namespace DoctorLoan.Application.Models.Commons;

/// <summary>
/// Represents a generic operation result with data.
/// </summary>
/// <typeparam name="T">Type of the returned data.</typeparam>
public class Result<T> : Result
{
    /// <summary>
    /// Gets or sets the returned data.
    /// </summary>
    [JsonProperty("data")]
    public T Data { get; set; }

    public Result(T data)
    {
        Data = data;
    }

    public Result(T data, ServiceError error)
        : base(error)
    {
        Data = data;
    }

    public Result(ServiceError error)
        : base(error)
    {
    }
}

/// <summary>
/// Represents an operation result.
/// </summary>
public class Result
{
    /// <summary>
    /// Indicates whether the operation succeeded.
    /// </summary>
    [JsonProperty("succeeded")]
    public bool Succeeded => Error == null;

    /// <summary>
    /// Gets or sets the error information.
    /// </summary>
    [JsonProperty("error")]
    public ServiceError Error { get; set; }

    public Result()
    {
    }

    public Result(ServiceError error)
    {
        Error = error ?? ServiceError.DefaultError;
    }

    public static Result Failed(ServiceError error)
    {
        return new Result(error);
    }

    public static Result<T> Failed<T>(ServiceError error)
    {
        return new Result<T>(error);
    }

    public static Result<T> Failed<T>(string message)
    {
        return new Result<T>(ServiceError.WithCustomMessage(message));
    }

    public static Result<T> Failed<T>(T data, ServiceError error)
    {
        return new Result<T>(data, error);
    }

    public static Result<T> Failed<T>(T data)
    {
        return new Result<T>(data, ServiceError.DefaultError);
    }

    public static Result<T> Success<T>(T data)
    {
        return new Result<T>(data);
    }
}
