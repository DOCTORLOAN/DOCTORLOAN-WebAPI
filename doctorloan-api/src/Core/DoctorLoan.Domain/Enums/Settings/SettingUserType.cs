using System.ComponentModel;

namespace DoctorLoan.Domain.Enums.Settings;

public enum SettingUserType
{
    /// <summary>
    /// Point Wallet
    /// </summary>
    [Description("Point")]
    Point = 1,
    /// <summary>
    /// Scan for taking inventory
    /// </summary>
    [Description("Quét lấy hàng")]
    Inventory = 2,
    /// <summary>
    /// Warning for low inventory
    /// </summary>
    [Description("Cảnh báo tồn kho thấp")]
    Warning = 3
}