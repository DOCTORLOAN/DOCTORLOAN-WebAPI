using System.ComponentModel.DataAnnotations.Schema;
using DoctorLoan.Domain.Enums.Commons;
using DoctorLoan.Domain.Enums.Users;

namespace DoctorLoan.Domain.Entities.Users;

[Table("UserIdentities")]
public class UserIdentity : BaseEntityAudit<int>
{
    public int UserId { get; set; }
    public UserIdentityType Type { get; set; }
    public DateTime DOB { get; set; }
    public string IdentityNo { get; set; }
    public DateTime IssuedDate { get; set; }
    public string PlaceOfIssue { get; set; }
    public UserIdentityStatus Status { get; set; }
    public Gender Gender { get; set; }
    public string Note { get; set; }
    public string Nationality { get; set; }
    public DateTime? ExpiredDate { get; set; }

    public virtual ICollection<UserMedia> UserMedias { get; set; } = new List<UserMedia>();
    public virtual ICollection<UserIdentityLog> UserIdentityLogs { get; set; } = new List<UserIdentityLog>();
}
