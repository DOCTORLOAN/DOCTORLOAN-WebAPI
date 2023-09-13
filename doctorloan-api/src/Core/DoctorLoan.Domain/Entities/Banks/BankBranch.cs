using System.ComponentModel.DataAnnotations.Schema;
using DoctorLoan.Domain.Entities.Users;
using DoctorLoan.Domain.Interfaces;

namespace DoctorLoan.Domain.Entities.Banks;

[Table("BankBranchs")]
public class BankBranch : BaseEntityAudit<int>, ISoftDeleteEntity
{
    public int BankId { get; set; }
    public int BranchId { get; set; }
    public string Code { get; set; }
    public bool IsDelete { get; set; }

    public virtual Bank Bank { get; set; }
    public virtual Branch Branch { get; set; }

    public virtual ICollection<UserBankBranch> UserBankBranchs { get; set; } = new List<UserBankBranch>();
}
