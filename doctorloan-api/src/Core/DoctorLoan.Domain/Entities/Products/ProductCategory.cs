using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorLoan.Domain.Entities.Products;
[Table("ProductCategories")]
public class ProductCategory:BaseEntityAudit<int>
{
    public int ProductId { get; set; }
    public int CategoryId { get; set; }
    public virtual Product Product { get; set; }
    public virtual Category Category { get; set; }
}
