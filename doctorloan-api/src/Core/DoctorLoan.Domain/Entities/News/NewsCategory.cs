using System.ComponentModel.DataAnnotations.Schema;
using DoctorLoan.Domain.Enums.Commons;

namespace DoctorLoan.Domain.Entities.News;
[Table("NewsCategories")]
public class NewsCategory:BaseEntityAudit<int>
{   public int? ParentId { get; set; }
    public string Name { get; set; }
    public StatusEnum Status { get; set; }
    public string Slug { get; set; }
    public string Description { get; set; }
    public int Sort { get; set; }
    public bool IsDeleted { get; set; }
}
