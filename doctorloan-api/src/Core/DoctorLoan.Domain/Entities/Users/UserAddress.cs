using System.ComponentModel.DataAnnotations.Schema;
using DoctorLoan.Domain.Entities.Addresses;
using DoctorLoan.Domain.Enums.Users;
using DoctorLoan.Domain.Interfaces;

namespace DoctorLoan.Domain.Entities.Users;

[Table("UserAddresses")]
public class UserAddress : BaseEntityAudit<int>, ISoftDeleteEntity
{
    public int UserId { get; set; }
    public int AddressId { get; set; }

    public string FullName { get; set; }
    public string PhoneCode { get; set; }
    public string Phone { get; set; }
    public UserAddressType Type { get; set; }
    public string Remarks { get; set; }
    public bool IsDefault { get; set; }
    public bool IsDelete { get; set; }

    public virtual User User { get; set; }
    public virtual Address Address { get; set; }
}