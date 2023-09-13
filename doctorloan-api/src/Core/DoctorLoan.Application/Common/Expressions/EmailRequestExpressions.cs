using System.Linq.Expressions;
using DoctorLoan.Domain.Entities.Emails;
using DoctorLoan.Domain.Enums.Emails;

namespace DoctorLoan.Infrastructure.Common.Expressions;

public static class EmailRequestExpressions
{
    public static Expression<Func<EmailRequest, bool>> IsValidEmailOTP(string email, string code, EmailType type) => s =>
        s.Email == email && s.Code == code && s.Type == type
        && s.Expired != true && s.IsSuccess != true;

    public static Func<EmailRequest, bool> IsValidOTP(string code, EmailType type) => s =>
        s.Code == code && s.Type == type
        && s.Expired != true && s.IsSuccess != true;

    public static Expression<Func<EmailRequest, bool>> OTPSucceeded(string email, string code, EmailType type) => s =>
       s.Email == email && s.Code == code && s.Type == type
        && s.Expired == true && s.IsSuccess == true;
}
