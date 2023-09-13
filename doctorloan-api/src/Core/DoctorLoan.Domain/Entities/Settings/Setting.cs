using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorLoan.Domain.Entities.Settings;

[Table("Settings")]
public class Setting : BaseEntityAudit<int>
{
    public string Name { get; set; }
    public string Value { get; set; }
    public int SystemId { get; set; }
    public SystemSettingType SettingApp
    {
        get => (SystemSettingType)SystemId;
        set => SystemId = (int)value;
    }
    public override string ToString()
    {
        return Name;
    }
}

public enum SystemSettingType
{
    All = 0,
    MobileApp = 1,
    Webadmin = 2,
}