using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorLoan.Domain.Entities.Products;
[Table("EntityLanguages")]
public class EntityLanguage : BaseEntityAudit<int>
{
    public int EntityId { get; set; }
    public string EntityName { get; set; }
    public string PropertyName { get; set; }
    public string Value { get; set; }
    public int LanguageId { get; set; }
}
