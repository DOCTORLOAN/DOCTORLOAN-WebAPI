using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorLoan.Domain.Entities.News;
[Table("NewsTags")]
public class NewsTag:BaseEntityAudit<int>
{ 
    public string Name { get; set; }
}
