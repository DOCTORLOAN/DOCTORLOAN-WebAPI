using System.ComponentModel.DataAnnotations.Schema;
using DoctorLoan.Domain.Entities.Medias;
using DoctorLoan.Domain.Enums.Users;

namespace DoctorLoan.Domain.Entities.Users;

[Table("UserMedias")]
public class UserMedia : BaseEntityAudit<int>
{
    public int UserId { get; set; }
    public int? UserIdentityId { get; set; }
    public long MediaId { get; set; }
    public UserMediaType Type { get; set; }
    public bool IsFront { get; set; } = false;
    public bool IsBack { get; set; } = false;

    public virtual Media Media { get; set; }
    public virtual UserIdentity UserIdentity { get; set; }
}