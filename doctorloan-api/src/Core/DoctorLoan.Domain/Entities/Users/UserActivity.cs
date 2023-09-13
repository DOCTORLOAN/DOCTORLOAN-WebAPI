using System.ComponentModel.DataAnnotations.Schema;
using DoctorLoan.Domain.Enums.Users;
using DoctorLoan.Domain.Interfaces;

namespace DoctorLoan.Domain.Entities.Users;

[Table("UserActivities")]
public class UserActivity : BaseEntityAudit<int>, ISoftDeleteEntity
{
    public int UserId { get; set; }
    public UserActivityType ActivityType { get; set; }
    public bool IsDelete { get; set; }

    public virtual User User { get; set; }
}
