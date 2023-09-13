using System.ComponentModel;

namespace DoctorLoan.Domain.Enums.Users;

public enum UserActivityType
{
    [Description("Đăng nhập")]
    SignIn = 1,
    SignOut = 2,
    Comment = 3,
    Like = 4
}