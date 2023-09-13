using DoctorLoan.Application.Common.Mappings;
using DoctorLoan.Domain.Entities.Bookings;
using DoctorLoan.Domain.Enums.Bookings;
using DoctorLoan.Domain.Enums.Commons;

namespace DoctorLoan.Customer.Application.Features.Customers.Dtos;

public class CustomerBookingDto : IMapFrom<Booking>
{
    public int Id { get; set; }

    public string FullName { get; set; }

    public string Phone { get; set; }

    public BookingType Type { get; set; }

    public string Address { get; set; }

    public int BookingTimes { get; set; }
    public DateTime BookingDate { get; set; }
    public TimeOnly BookingStartTime { get; set; }
    public TimeOnly BookingEndTime { get; set; }
    public BookingStatus Status { get; set; }
}
