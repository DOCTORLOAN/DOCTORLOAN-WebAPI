using Newtonsoft.Json;

namespace DoctorLoan.Application.Models.Commons;

public class Result<T> : Result
{
    [JsonProperty("data")]
    public T Data { get; set; }

    public Result(T data)
    {
        Data = data;
    }

    public Result(T data, ServiceError error) : base(error)
    {
        Data = data;
    }

    public Result(ServiceError error) : base(error)
    {

    }
}

public class Result
{
    [JsonProperty("succeeded")]
    public bool Succeeded => this.Error == null;

    [JsonProperty("error")]
    public ServiceError Error { get; set; }

    public Result(ServiceError error)
    {
        if (error == null)
        {
            error = ServiceError.DefaultError;
        }

        Error = error;
    }

    public Result() { }

    #region Helper Methods
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
    #endregion
}
