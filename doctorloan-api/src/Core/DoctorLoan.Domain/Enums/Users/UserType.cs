using System.ComponentModel;

namespace DoctorLoan.Domain.Enums.Users;

public enum UserType
{
    [Description("Bán lẻ")]
    Retail = 1,

    [Description("CTV")]
    CTV = 2,

    [Description("Đại lý")]
    DaiLy = 3,

    [Description("Khách mua hàng")]
    Customer = 4,

    [Description("Trainer Chili")]
    Trainer = 5,

    [Description("Student")]
    Student = 6
}