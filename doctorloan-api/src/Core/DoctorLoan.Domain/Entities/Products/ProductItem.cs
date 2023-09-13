using System.ComponentModel.DataAnnotations.Schema;
using DoctorLoan.Domain.Entities.Orders;

namespace DoctorLoan.Domain.Entities.Products;
[Table("ProductItems")]
public class ProductItem : BaseEntityAudit<int>
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string Sku { get; set; }
    public decimal PriceDiscount { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int Sold { get; set; }
    public int Available { get; set; }
    public int Defective { get; set; }
    public virtual Product Product { get; set; }


    public virtual ICollection<ProductOption> ProductOptions { get; set; }

    public virtual List<OrderItem> OrderItems { get; set; }
}
