using Newtonsoft.Json;

namespace DoctorLoan.Application.Common.Exceptions;
public class DomainException : Exception
{
    public DomainException(string key, string message)
        : base(message)
    {
        Key = key;
    }

    public string Key { get; set; }

    public static DomainException BuildNotExistException(string message)
    {
        return new DomainException("NOT_EXIST", message);
    }
}

public class DomainExceptionContract
{
    public string Key { get; set; }

    public string Message { get; set; }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}