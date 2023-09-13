using System.ComponentModel.DataAnnotations.Schema;
using DoctorLoan.Domain.Entities.Users;

namespace DoctorLoan.Domain.Entities.Authorizations;

[Table("UserPermissions")]
public class UserPermission : BaseEntityAudit<long>
{
    public int UserId { get; set; }
    public int PermissionActionId { get; set; }


    public virtual User User { get; set; }
    public virtual PermissionAction PermissionAction { get; set; }
}
