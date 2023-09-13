using System.Text.Json.Serialization;
using DoctorLoan.Application.Common.Mappings;
using DoctorLoan.Domain.Entities.Customers;
using DoctorLoan.Domain.Enums.Commons;

namespace DoctorLoan.Customer.Application.Features.Customers;


public class CustomerDto : IMapFrom<Domain.Entities.Customers.Customer>
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string FullName { get; set; }

    public string Email { get; set; }

    public string Phone { get; set; }

    public Gender Gender { get; set; }

    public DateTimeOffset? DOB { get; set; }

    public DateTimeOffset Created { get; set; }

    [JsonIgnore]
    public List<CustomerAddress>? CustomerAddresses { get; set; }
}

