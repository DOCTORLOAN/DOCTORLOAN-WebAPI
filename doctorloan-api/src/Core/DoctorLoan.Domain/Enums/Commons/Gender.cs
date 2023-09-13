using System.ComponentModel;

namespace DoctorLoan.Domain.Enums.Commons;

public enum Gender
{
    [Description("Nam")]
    Male = 1,
    [Description("Nữ")]
    Female = 2,
    [Description("Khác")]
    Others = 3
}
