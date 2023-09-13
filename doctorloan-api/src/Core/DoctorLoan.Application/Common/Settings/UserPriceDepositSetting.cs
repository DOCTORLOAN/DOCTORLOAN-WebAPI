using DoctorLoan.Domain.Interfaces;

namespace DoctorLoan.Application.Common.Settings;

public class UserPriceDepositSetting : ISettings
{
    public decimal MaxCOD { get; set; }
    public decimal MinPercentDeposit { get; set; }
    public decimal CTVDeposit { get; set; }
    public decimal DaiLyDeposit { get; set; }
}