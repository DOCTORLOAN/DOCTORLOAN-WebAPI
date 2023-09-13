using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorLoan.Domain.Entities.Users;

[Table("UserDevices")]
public class UserDevice : BaseEntityAudit<int>
{
    public int UserId { get; set; }
    public int DeviceId { get; set; }
    public bool IsActive { get; set; }

    public virtual Device Device { get; set; }
    public virtual User User { get; set; }
}