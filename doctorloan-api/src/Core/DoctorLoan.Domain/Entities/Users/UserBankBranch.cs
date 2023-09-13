using System.ComponentModel.DataAnnotations.Schema;
using DoctorLoan.Domain.Entities.Banks;
using DoctorLoan.Domain.Enums.Users;
using DoctorLoan.Domain.Interfaces;

namespace DoctorLoan.Domain.Entities.Users;

[Table("UserBankBranchs")]
public class UserBankBranch : BaseEntityAudit<int>, ISoftDeleteEntity
{
    public int UserId { get; set; }
    public int BankBranchId { get; set; }
    public string AccountNo { get; set; }
    public string AccountName { get; set; }
    public bool IsPrimary { get; set; }
    public UserBankBranchStatus Status { get; set; }
    public bool IsDelete { get; set; }
    public bool IsDedicated { get; set; }
    public string Note { get; set; }

    public virtual BankBranch BankBranch { get; set; }
    public virtual User User { get; set; }
}
