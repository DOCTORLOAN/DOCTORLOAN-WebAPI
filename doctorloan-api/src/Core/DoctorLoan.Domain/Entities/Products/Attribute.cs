using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorLoan.Domain.Entities.Products;
[Table("Attributes")]
public class Attribute : BaseEntityAudit<int>
{
    public string Name { get; set; }
    public int GroupId { get; set; }
    public virtual AttributeGroup AttributeGroup { get; set; }
}
