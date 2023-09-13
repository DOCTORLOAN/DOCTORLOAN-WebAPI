using System.ComponentModel.DataAnnotations.Schema;
using DoctorLoan.Domain.Enums.Settings;

namespace DoctorLoan.Domain.Entities.Settings;

[Table("SettingUserLogs")]
public class SettingUserLog : BaseEntityAudit<int>
{
    public int UserId { get; set; }
    public int SettingUserId { get; set; }
    public SettingUserType Type { get; set; }
    public bool IsAgree { get; set; }
}