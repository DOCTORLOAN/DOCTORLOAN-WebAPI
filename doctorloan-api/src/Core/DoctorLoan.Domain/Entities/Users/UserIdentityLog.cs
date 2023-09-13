using System.ComponentModel.DataAnnotations.Schema;
using DoctorLoan.Domain.Enums.Users;

namespace DoctorLoan.Domain.Entities.Users;

[Table("UserIdentityLogs")]
public class UserIdentityLog : BaseEntityAudit<int>
{
    public int UserIdentityId { get; set; }
    public int UserId { get; set; }
    public UserIdentityType Type { get; set; }
    public DateTime DOB { get; set; }
    public string IdentityNo { get; set; }
    public DateTime IssuedDate { get; set; }
    public string PlaceOfIssue { get; set; }
    public UserIdentityStatus Status { get; set; }
    public string Note { get; set; }

    public virtual UserIdentity UserIdentity { get; set; }
}