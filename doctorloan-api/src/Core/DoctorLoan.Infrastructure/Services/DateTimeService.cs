
using DoctorLoan.Application.Interfaces.Commons;

namespace DoctorLoan.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
