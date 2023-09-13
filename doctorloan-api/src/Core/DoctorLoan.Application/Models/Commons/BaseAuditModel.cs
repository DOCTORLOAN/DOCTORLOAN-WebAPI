namespace DoctorLoan.Application.Models.Commons;
public class BaseAuditModel
{
    public DateTimeOffset Created { get; set; }
    public int CreatedBy { get; set; }
    public DateTimeOffset? LastModified { get; set; }
    public int? LastModifiedBy { get; set; }
    public string CreatedByName { get; set; }
    public string LastModifiedByName { get; set; }
}
