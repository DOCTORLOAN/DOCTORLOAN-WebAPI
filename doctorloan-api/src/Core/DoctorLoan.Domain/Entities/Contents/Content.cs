using System.ComponentModel.DataAnnotations.Schema;
using DoctorLoan.Domain.Entities.Medias;
using DoctorLoan.Domain.Enums.Commons;
using DoctorLoan.Domain.Enums.Contents;

namespace DoctorLoan.Domain.Entities.Contents;

[Table("Contents")]
public class Content : BaseEntityAudit<int>
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public StatusEnum Status { get; set; }
    public long? MediaId { get; set; }

    public ContentTypeEnum Type { get; set; }
    public virtual Media Media { get; set; }
}