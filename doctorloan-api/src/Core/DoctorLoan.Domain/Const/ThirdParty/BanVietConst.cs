namespace DoctorLoan.Domain.Const.ThirdParty;

public static class BanVietAccountType
{
    /// <summary>
    /// : Chỉ nhận tiền một lần.
    /// </summary>
    public const string One_Time = "O";

    /// <summary>
    /// Nhận tiền không giới hạn.
    /// </summary>
    public const string Many_Time = "M";
}

public static class BanVietEventName
{
    /// <summary>
    /// : Chỉ nhận tiền một lần.
    /// </summary>
    public const string BalanceUpdate = "BALANCE_UPDATE";
}

public static class BanVietDRCR
{
    public const string Debit = "D";
    public const string Credit = "C";
}

