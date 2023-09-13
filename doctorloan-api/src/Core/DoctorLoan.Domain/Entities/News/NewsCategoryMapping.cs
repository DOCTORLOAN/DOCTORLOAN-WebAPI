using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorLoan.Domain.Entities.News;

[Table("NewsCategoryMappings")]
public class NewsCategoryMapping : BaseEntityAudit<int>
{
    public int NewsItemId { get; set; }
    public int NewsCategoryId { get; set; }
    public virtual NewsCategory NewsCategory { get; set; }
    public virtual NewsItem NewsItem { get; set; }
}
