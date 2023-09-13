using System.ComponentModel.DataAnnotations.Schema;
using DoctorLoan.Domain.Interfaces;

namespace DoctorLoan.Domain.Entities.Addresses;

[Table("Districts")]
public class District : BaseEntityAudit<int>, ISoftDeleteEntity
{
    public string Name { get; set; }
    public string Code { get; set; }
    public string Type { get; set; }
    public decimal? Longitude { get; set; }
    public decimal? Latitude { get; set; }
    public int ProvinceId { get; set; }
    public int? SortBy { get; set; }
    public bool IsPublished { get; set; }
    public bool IsDelete { get; set; }

    public virtual ICollection<Ward> Wards { get; set; } = new List<Ward>();
    public virtual Province Province { get; set; }
    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
}