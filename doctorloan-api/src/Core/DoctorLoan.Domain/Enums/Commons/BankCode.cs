using System.ComponentModel;

namespace DoctorLoan.Domain.Enums.Users;

public enum BankCode
{
    /// <summary>
    /// MB Bank
    /// </summary>
    [Description("MB BANK")]
    MBB = 1,

    /// <summary>
    // Techcombank
    /// </summary>
    [Description("TC BANK")]
    TCB = 2,

    /// <summary>
    // Bản Việt Bank
    /// </summary>
    [Description("BV BANK")]
    BVB = 3,

    /// <summary>
    // Vietcombank
    /// </summary>
    /// 
    [Description("Vietcombank")]
    VCB = 4,

    /// <summary>
    // BIDV
    /// </summary>
    [Description("BIDV")]
    BIDV = 5,

    /// <summary>
    // GPAY
    /// </summary>
    [Description("GPAY")]
    GPAY = 77
}
