using System.ComponentModel.DataAnnotations.Schema;
using DoctorLoan.Domain.Entities.Customers;
using DoctorLoan.Domain.Enums.Bookings;
using DoctorLoan.Domain.Enums.Commons;

namespace DoctorLoan.Domain.Entities.Bookings;

[Table("Bookings")]
public class Booking : BaseEntityAudit<int>
{
    public BookingType Type { get; set; }
    public int CustomerId { get; set; }
    public int? CustomerAddressId { get; set; }
    public int BookingTimes { get; set; }
    public DateOnly BookingDate { get; set; }
    public TimeOnly BookingStartTime { get; set; }
    public TimeOnly BookingEndTime { get; set; }
    public BookingStatus Status { get; set; }
    public string Noted { get; set; }

    public virtual Customer Customer { get; set; }
    public virtual CustomerAddress CustomerAddresses { get; set; }

}