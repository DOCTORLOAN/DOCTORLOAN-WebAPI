using System.ComponentModel.DataAnnotations.Schema;
using DoctorLoan.Domain.Interfaces;

namespace DoctorLoan.Domain.Entities.Banks;

[Table("Banks")]
public class Bank : BaseEntityAudit<int>, ISoftDeleteEntity
{
    public string Name { get; set; }
    public string Code { get; set; }
    public bool IsDelete { get; set; }

    public virtual ICollection<BankBranch> BankBranchs { get; set; } = new List<BankBranch>();
}
