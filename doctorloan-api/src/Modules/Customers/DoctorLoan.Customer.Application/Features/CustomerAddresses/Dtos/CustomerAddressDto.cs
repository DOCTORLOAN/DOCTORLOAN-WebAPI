using DoctorLoan.Application.Common.Mappings;
using DoctorLoan.Domain.Entities.Customers;
using DoctorLoan.Domain.Enums.Addresses;

namespace DoctorLoan.Customer.Application.Features.CustomerAddresses.Dtos;

public class CustomerAddressDto : IMapFrom<CustomerAddress>
{
    public int Id { get; set; }
    public AddressType Type { get; set; }
    public string FullName { get; set; }
    public string Phone { get; set; }
    public string Remarks { get; set; }
    public bool IsDefault { get; set; }
    public string Address { get; set; }
}
