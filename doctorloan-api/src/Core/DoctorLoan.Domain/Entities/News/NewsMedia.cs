using System.ComponentModel.DataAnnotations.Schema;
using DoctorLoan.Domain.Entities.Medias;

namespace DoctorLoan.Domain.Entities.News;
[Table("NewsMedias")]
public class NewsMedia:BaseEntityAudit<int>
{
    public bool IsThumb { get; set; }
    public int NewsId { get; set; }
    public long MediaId { get; set; }
    public int OrderBy { get; set; }
    public virtual Media Media { get; set; }
    public virtual NewsItem NewsItem { get; set; }
}
