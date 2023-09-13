using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorLoan.Domain.Entities.Products;
[Table("Brands")]
public class Brand:BaseEntityAudit<int>
{
    public string Name { get; set; }
    public string Summary { get; set; }
}
