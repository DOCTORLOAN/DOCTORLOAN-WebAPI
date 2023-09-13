using DoctorLoan.Domain.Const.Commons;
using DoctorLoan.Domain.Enums.Users;

namespace DoctorLoan.Application.Common.Extentions;

public static class ValidationExtention
{
    public static bool IsForeign(string phoneCode)
    {
        return !string.IsNullOrEmpty(phoneCode) && phoneCode.Replace("+", "") != PhoneCodeConsts.VN;
    }

    public static bool IsUserBlocked(UserStatus status)
    {
        var listStatusInActive = new List<UserStatus>() { UserStatus.Block, UserStatus.Removed, UserStatus.Deleted };
        return listStatusInActive.Contains(status);
    }

    public static bool BeAValidAge(DateTime date)
    {
        var now = DateTime.Now;
        int dobYear = date.Year;

        if (dobYear < (now.Year - 18))
        {
            return dobYear > (now.Year - 120);
        }
        else if (dobYear <= (now.Year - 18))
        {
            return date.DayOfYear >= now.DayOfYear && dobYear > (now.Year - 120);
        }

        return false;
    }
}
