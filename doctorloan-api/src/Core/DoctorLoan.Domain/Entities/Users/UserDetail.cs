
using System.ComponentModel.DataAnnotations.Schema;
using DoctorLoan.Domain.Entities.Commons;

namespace DoctorLoan.Domain.Entities.Users;

[Table("UserDetails")]
public class UserDetail : BaseEntityAudit<int>
{
    public int UserId { get; set; }
    public int? JobId { get; set; }
    public string Facebook { get; set; }
    public string Tiktok { get; set; }
    public string Zalo { get; set; }
    public int CountryId { get; set; }
    public DateTimeOffset? LastedSignIn { get; set; }

    public virtual Job Job { get; set; }
}