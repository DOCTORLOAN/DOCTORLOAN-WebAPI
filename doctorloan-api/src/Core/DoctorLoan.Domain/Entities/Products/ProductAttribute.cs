using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorLoan.Domain.Entities.Products;
[Table("ProductAttributes")]
public class ProductAttribute : BaseEntityAudit<int>
{
    public int ProductId { get; set; }
    public int AttributeId { get; set; }
    public string Value { get; set; }
    public virtual Product Product { get; set; }
    public virtual Attribute Attribute { get; set; }
}