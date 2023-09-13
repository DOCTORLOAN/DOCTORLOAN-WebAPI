namespace DoctorLoan.Application.Interfaces.Commons;

public interface IDateTime
{
    DateTimeOffset UtcNow { get; }
}
