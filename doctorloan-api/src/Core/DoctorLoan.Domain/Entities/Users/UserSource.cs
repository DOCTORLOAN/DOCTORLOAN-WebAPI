using System.ComponentModel.DataAnnotations.Schema;
using DoctorLoan.Domain.Interfaces;

namespace DoctorLoan.Domain.Entities.Users;
[Table("UserSources")]
public class UserSource : BaseEntityAudit<int>, ISoftDeleteEntity
{
    public string Description { get; set; }
    public string Name { get; set; }
    public int? ParentId { get; set; }
    public int? EventId { get; set; }
    public bool IsDelete { get; set; }
}
