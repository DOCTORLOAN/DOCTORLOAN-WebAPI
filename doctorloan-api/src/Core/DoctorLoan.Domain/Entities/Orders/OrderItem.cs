using System.ComponentModel.DataAnnotations.Schema;
using DoctorLoan.Domain.Entities.Products;

namespace DoctorLoan.Domain.Entities.Orders;

[Table("OrderItems")]
public class OrderItem : BaseEntityAudit<int>
{
    public int OrderId { get; set; }
    public int ProductItemId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }

    public virtual Order Order { get; set; }
    public virtual ProductItem ProductItem { get; set; }
}
