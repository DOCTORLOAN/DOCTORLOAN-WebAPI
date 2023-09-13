using System.ComponentModel.DataAnnotations.Schema;
using DoctorLoan.Domain.Enums.Commons;

namespace DoctorLoan.Domain.Entities.Products;
[Table("Products")]
public class Product : BaseEntityAudit<int>
{
    public string Name { get; set; }
    public string Sku { get; set; }
    public StatusEnum Status { get; set; }
    public string Slug { get; set; }
    public int BrandId { get; set; }
    public decimal Price { get; set; } = 0;
    public decimal PriceDiscount { get; set; } = 0;
    public int? Quantity { get; set; }
    public virtual ICollection<ProductItem> ProductItems { get; set; } = new HashSet<ProductItem>();
    public virtual ICollection<ProductAttribute> ProductAttributes { get; set; } = new HashSet<ProductAttribute>();
    public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new HashSet<ProductDetail>();
    public virtual ICollection<ProductMedia> ProductMedias { get; set; } = new HashSet<ProductMedia>();
    public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new HashSet<ProductCategory>();
    public virtual Brand Brand { get; set; }
    public bool IsDelete { get; set; }
}
