using System.ComponentModel.DataAnnotations.Schema;
using DoctorLoan.Domain.Enums.Commons;

namespace DoctorLoan.Domain.Entities.Users;


[Table("Devices")]
public class Device : BaseEntityAudit<int>
{

    public PlatformType Platform { get; set; }

    public string DeviceId { get; set; }

    public string DeviceName { get; set; }

    public string Version { get; set; }

    public string VersionApp { get; set; }

    public string Object { get; set; }

    public string IsActive { get; set; }


    public virtual ICollection<UserDevice> UserDevices { get; set; } = new List<UserDevice>();
}