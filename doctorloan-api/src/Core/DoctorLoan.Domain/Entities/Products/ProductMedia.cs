using System.ComponentModel.DataAnnotations.Schema;
using DoctorLoan.Domain.Entities.Medias;
using DoctorLoan.Domain.Enums.Commons;

namespace DoctorLoan.Domain.Entities.Products;
[Table("ProductMedias")]
public class ProductMedia : BaseEntityAudit<int>
{
    public long MediaId { get; set; }
    public int ProductId { get; set; }
    public int OrderBy { get; set; }
    public StatusEnum Status { get; set; }
    public bool IsDelete { get; set; }
    public int? ProductItemId { get; set; }
    public virtual Product Product { get; set; }
    public virtual Media Media { get; set; }
}
