using System.ComponentModel.DataAnnotations.Schema;
using DoctorLoan.Domain.Entities.Departments;
using DoctorLoan.Domain.Entities.Users;

namespace DoctorLoan.Domain.Entities.Roles;

[Table("Roles")]
public class Role : BaseEntityAudit<int>
{
    public int DepartmentId { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public int? Level { get; set; }
    public int OrderBy { get; set; }
    public bool IsActive { get; set; }

    public virtual Department Department { get; set; }
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
