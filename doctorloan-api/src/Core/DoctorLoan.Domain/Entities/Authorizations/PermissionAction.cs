using System.ComponentModel.DataAnnotations.Schema;
using DoctorLoan.Domain.Interfaces;

namespace DoctorLoan.Domain.Entities.Authorizations;

[Table("PermissionActions")]
public class PermissionAction : BaseEntity<int>, ISoftDeleteEntity
{
    public int ModuleId { get; set; }
    public int ActionId { get; set; }
    public bool IsDelete { get; set; }

    public virtual ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
}
