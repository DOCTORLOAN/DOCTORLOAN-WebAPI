using System.ComponentModel.DataAnnotations.Schema;
using DoctorLoan.Domain.Enums.Addresses;
using DoctorLoan.Domain.Interfaces;

namespace DoctorLoan.Domain.Entities.Addresses;

[Table("Provinces")]
public class Province : BaseEntityAudit<int>, ISoftDeleteEntity
{
    public string Name { get; set; }
    public string Code { get; set; }
    public string Type { get; set; }
    public int? PhoneCode { get; set; }
    public string ZipCode { get; set; }
    public int CountryId { get; set; }
    public RegionType? RegionType { get; set; }
    public int? SortBy { get; set; }
    public bool IsPublished { get; set; }
    public bool IsDelete { get; set; }

    public virtual Country Country { get; set; }
    public virtual ICollection<District> Districts { get; set; } = new List<District>();
    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
}