using System.ComponentModel.DataAnnotations.Schema;
using DoctorLoan.Domain.Entities.Roles;

namespace DoctorLoan.Domain.Entities.Departments;

[Table("Departments")]
public class Department : BaseEntityAudit<int>
{
    public string Code { get; set; }
    public string Name { get; set; }
    public int OrderBy { get; set; }
    public bool IsActive { get; set; }

    public virtual ICollection<Role> DepartmentRoles { get; } = new HashSet<Role>();
}