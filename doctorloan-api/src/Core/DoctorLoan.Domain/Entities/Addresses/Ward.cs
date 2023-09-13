using System.ComponentModel.DataAnnotations.Schema;
using DoctorLoan.Domain.Interfaces;

namespace DoctorLoan.Domain.Entities.Addresses;

[Table("Wards")]
public class Ward : BaseEntityAudit<int>, ISoftDeleteEntity
{
    public string Name { get; set; }
    public string Code { get; set; }
    public string Type { get; set; }
    public decimal? Longitude { get; set; }
    public decimal? Latitude { get; set; }
    public int DistrictId { get; set; }
    public int? SortBy { get; set; }
    public bool IsPublished { get; set; }
    public bool IsDelete { get; set; }

    public virtual District District { get; set; }
    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
}