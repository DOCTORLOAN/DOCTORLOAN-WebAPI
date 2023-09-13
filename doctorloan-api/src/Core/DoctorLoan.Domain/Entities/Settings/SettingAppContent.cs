using System.ComponentModel.DataAnnotations.Schema;
using DoctorLoan.Domain.Enums.Commons;

namespace DoctorLoan.Domain.Entities.Settings;

[Table("SettingAppContents")]
public class SettingAppContent : BaseEntityAudit<int>
{
    public int SettingAppId { get; set; }
    public LanguageEnum Language { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }

    public virtual SettingApp SettingApp { get; set; }
}
