using System.ComponentModel.DataAnnotations.Schema;
using DoctorLoan.Domain.Interfaces;

namespace DoctorLoan.Domain.Entities.Addresses;

[Table("Countries")]
public class Country : BaseEntityAudit<int>, ISoftDeleteEntity
{
    public string Code { get; set; }
    public string Code3 { get; set; }
    public string Name { get; set; }
    public string FormalName { get; set; }
    public string CountryType { get; set; }
    public string CountrySubType { get; set; }
    public string Sovereignty { get; set; }
    public string Capital { get; set; }
    public string CurrencyCode { get; set; }
    public string CurrencyName { get; set; }
    public string PhoneCode { get; set; }
    public int? CountryNo { get; set; }
    public string InternetCountryCode { get; set; }
    public string Flags { get; set; }
    public int? SortBy { get; set; }
    public bool IsPublished { get; set; }
    public bool IsDelete { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
    public virtual ICollection<Province> Provinces { get; set; } = new List<Province>();
}