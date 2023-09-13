namespace DoctorLoan.Domain.Entities.Commons;
public class Language : BaseEntityAudit<int>
{
    public string LanguageCode { get; set; }
    public string LanguageName { get; set; }
    public int OrderBy { get; set; }
    public int Status { get; set; }
    public bool IsDefault { get; set; }
}
