using System.ComponentModel.DataAnnotations.Schema;
using DoctorLoan.Domain.Enums.Emails;

namespace DoctorLoan.Domain.Entities.Emails;

[Table("EmailRequests")]
public class EmailRequest : BaseEntityAudit<int>
{
    public int? UserId { get; set; }
    public string Email { get; set; }
    public EmailType Type { get; set; }
    public string Code { get; set; }
    public bool IsSuccess { get; set; }
    public bool Expired { get; set; } //  When user verify success -> update all request = expired
    public string DeviceId { get; set; }
    public string OSBrowser { get; set; }
}
