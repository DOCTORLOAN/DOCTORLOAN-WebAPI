using System.ComponentModel.DataAnnotations.Schema;
using DoctorLoan.Domain.Enums.Commons;

namespace DoctorLoan.Domain.Entities.News;
[Table("NewsItems")]
public class NewsItem:BaseEntityAudit<int>
{
    public string Title { get; set; }
    public StatusEnum Status{ get; set; }
    public string Slug { get; set; }
    public virtual ICollection<NewsItemDetail> NewsItemDetails { get; set; } = new HashSet<NewsItemDetail>();
    public virtual ICollection<NewsMedia> NewsMedias { get; set; }=new HashSet<NewsMedia>();
    public virtual ICollection<NewsTagsMapping> NewsTags { get; set; } = new HashSet<NewsTagsMapping>();
    public virtual ICollection<NewsCategoryMapping> NewsCategories { get; set; } = new HashSet<NewsCategoryMapping>();
    public bool IsDeleted { get; set; }
}
