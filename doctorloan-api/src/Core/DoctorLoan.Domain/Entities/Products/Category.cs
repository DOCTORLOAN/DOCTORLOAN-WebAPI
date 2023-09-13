using System.ComponentModel.DataAnnotations.Schema;
using DoctorLoan.Domain.Enums.Commons;
using Microsoft.EntityFrameworkCore;

namespace DoctorLoan.Domain.Entities.Products;


[Table("Categories")]
public class Category : BaseEntityAudit<int>
{
    public string Name { get; set; }
    public string Code { get; set; }
    public int ParentId { get; set; }
    public string MetaTitle { get; set; }
    public string Content { get; set; }
    public int Sort { get; set; }
    public string Slug { get; set; }
    public StatusEnum Status { get; set; } = StatusEnum.Draft;
    public bool IsDeleted { get; set; }
    public ICollection<ProductCategory> ProductCategories { get; set; }
}
