using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorLoan.Domain.Entities.Products;
[Table("ProductDetails")]
public class ProductDetail : BaseEntityAudit<int>
{
    public int ProductId { get; set; }
    public int LanguageId { get; set; }
    public string Description { get; set; }
    public string Summary { get; set; }
    public string MetadataKeyword { get; set; }
    public string MetadataTitle { get; set; }
    public string MetadataDesc { get; set; }
    public virtual Product Product { get; set; }
}
