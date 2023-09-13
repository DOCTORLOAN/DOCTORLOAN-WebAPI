using System.ComponentModel.DataAnnotations.Schema;
using DoctorLoan.Domain.Enums.Settings;

namespace DoctorLoan.Domain.Entities.Settings;

[Table("SettingApps")]
public class SettingApp : BaseEntityAudit<int>
{
    public AppSetupType Type { get; set; }
    public string AppVersion { get; set; }
    public string PrefixCode { get; set; }
    public string PrefixChildCode { get; set; }
    public string KeyValue { get; set; }
    public bool IsActive { get; set; }

    public virtual ICollection<SettingAppContent> SettingAppContents { get; } = new HashSet<SettingAppContent>();
}