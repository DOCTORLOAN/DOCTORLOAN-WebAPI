using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorLoan.Domain.Entities.MedicalRecord;

[Table("Symptoms")]
public class Symptoms : BaseEntityAudit<int>
{
    public string Name { get; set; }
    public int? SymptomGroupId { get; set; }
    public int parentId { get; set; }
    public bool IsDelete { get; set; }
    public virtual SymptomGroups SymptomGroup { get; set; }
}
