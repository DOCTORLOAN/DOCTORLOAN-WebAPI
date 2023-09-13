using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorLoan.Domain.Entities.Products;

[Table("AttributeGroups")]
public class AttributeGroup : BaseEntityAudit<int>
{
    public string Name { get; set; }
    public virtual ICollection<Attribute> Attributes { get; set; }
}
