using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using DoctorLoan.Domain.Entities.Customers;
using DoctorLoan.Domain.Entities.Users;

namespace DoctorLoan.Domain.Entities.Addresses;

[Table("Addresses")]
public class Address : BaseEntityAudit<int>
{
    public string AddressLine { get; set; }
    public int CountryId { get; set; }
    public int? ProvinceId { get; set; }
    public int? DistrictId { get; set; }
    public int? WardId { get; set; }

    [Description("Indicate how many tables, such as the user, customer, and order tables, are utilizing this address...")]
    public int? TotalRelated { get; set; }

    public virtual Country Country { get; set; }
    public virtual Province Province { get; set; }
    public virtual District District { get; set; }
    public virtual Ward Ward { get; set; }


    public virtual ICollection<UserAddress> UserAddresses { get; set; } = new List<UserAddress>();
    public virtual ICollection<CustomerAddress> CustomerAddresses { get; set; } = new List<CustomerAddress>();
}
