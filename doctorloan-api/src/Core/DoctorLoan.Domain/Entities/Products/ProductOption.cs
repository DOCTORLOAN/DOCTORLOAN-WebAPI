using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorLoan.Domain.Entities.Products;
[Table("ProductOptions")]
public class ProductOption : BaseEntityAudit<int>
{
    public int ProductItemId { get; set; }
    public int OptionGroupId { get; set; }
    public string Name { get; set; }
    public string DisplayValue { get; set; }
    public virtual ProductItem ProductItem { get; set; }
    public virtual ProductOptionGroup OptionGroup { get; set; }
}
