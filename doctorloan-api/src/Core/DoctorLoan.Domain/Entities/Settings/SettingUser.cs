using System.ComponentModel.DataAnnotations.Schema;
using DoctorLoan.Domain.Entities.Users;
using DoctorLoan.Domain.Enums.Settings;
using DoctorLoan.Domain.Interfaces;

namespace DoctorLoan.Domain.Entities.Settings;

[Table("SettingUsers")]
public class SettingUser : BaseEntityAudit<int>, ISoftDeleteEntity
{
    public int UserId { get; set; }
    public SettingUserType Type { get; set; }
    public bool IsAgree { get; set; }
    public bool IsDelete { get; set; }
    public virtual User Users { get; set; }
    public virtual ICollection<SettingUserLog> SettingUserLogs { get; set; } = new List<SettingUserLog>();
}