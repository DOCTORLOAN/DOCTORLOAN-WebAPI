using DoctorLoan.Application.Common.Mappings;
using DoctorLoan.Domain.Enums.Bookings;
using DoctorLoan.Domain.Enums.Commons;

namespace DoctorLoan.Booking.Application.Features.Dtos;

public class BookingDto : IMapFrom<Domain.Entities.Bookings.Booking>
{
    public int Id { get; set; }

    public string FullName { get; set; }

    public string Phone { get; set; }

    public BookingType Type { get; set; }

    public string Address { get; set; }

    public int BookingTimes { get; set; }
    public DateOnly BookingDate { get; set; }
    public TimeOnly BookingStartTime { get; set; }
    public TimeOnly BookingEndTime { get; set; }
    public BookingStatus Status { get; set; }
    public string Noted { get; set; }
}
