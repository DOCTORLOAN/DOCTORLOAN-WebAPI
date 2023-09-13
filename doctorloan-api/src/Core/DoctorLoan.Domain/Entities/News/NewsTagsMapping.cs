using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorLoan.Domain.Entities.News;

[Table("NewsTagsMappings")]
public class NewsTagsMapping : BaseEntityAudit<int>
{
    public int NewsItemId { get; set; }
    public int NewsTagId { get; set; }
    public virtual NewsItem NewsItem { get; set; }
    public virtual NewsTag NewsTag { get; set; }
}
