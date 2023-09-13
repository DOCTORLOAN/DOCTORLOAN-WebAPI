using System.ComponentModel.DataAnnotations.Schema;
using DoctorLoan.Domain.Enums.Commons;
using DoctorLoan.Domain.Interfaces;

namespace DoctorLoan.Domain.Entities.Customers;

[Table("Customers")]
public class Customer : BaseEntityAudit<int>, ISoftDeleteEntity
{
    public Guid UID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public Gender Gender { get; set; }
    public DateTimeOffset? DOB { get; set; }
    public string PasswordHash { get; set; }

    public bool IsDelete { get; set; }

    public virtual ICollection<CustomerAddress> CustomerAddresses { get; set; } = new List<CustomerAddress>();

}