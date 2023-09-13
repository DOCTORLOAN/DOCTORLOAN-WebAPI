using System.ComponentModel.DataAnnotations.Schema;
using DoctorLoan.Domain.Entities.Addresses;
using DoctorLoan.Domain.Enums.Addresses;
using DoctorLoan.Domain.Interfaces;

namespace DoctorLoan.Domain.Entities.Customers;

[Table("CustomerAddresses")]
public class CustomerAddress : BaseEntityAudit<int>, ISoftDeleteEntity
{
    public int CustomerId { get; set; }
    public int AddressId { get; set; }
    public AddressType Type { get; set; }

    public string FullName { get; set; }
    public string Phone { get; set; }
    public string Remarks { get; set; }
    public bool IsDefault { get; set; }
    public bool IsDelete { get; set; }

    public virtual Customer Customer { get; set; }
    public virtual Address Address { get; set; }
}
