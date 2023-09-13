using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorLoan.Domain.Entities.Products;
[Table("ProductOptionGroups")]
public class ProductOptionGroup : BaseEntityAudit<int>
{
    public string Name { get; set; }
}