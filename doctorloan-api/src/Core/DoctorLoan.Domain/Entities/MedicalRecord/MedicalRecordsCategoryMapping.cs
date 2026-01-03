using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorLoan.Domain.Entities.MedicalRecord;

[Table("MedicalRecordsCategoryMapping")]
public class MedicalRecordsCategoryMapping : BaseEntityAudit<int>
{
    public int MedicalRecordId { get; set; }
    public int MedicalRecordsCategoryId { get; set; }
    public virtual MedicalRecordsCategory MedicalRecordsCategory { get; set; }
    public virtual MedicalRecord MedicalRecord { get; set; }
}