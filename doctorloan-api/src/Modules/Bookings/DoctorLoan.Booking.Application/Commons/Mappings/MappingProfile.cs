using AutoMapper;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Booking.Application.Features.Commands;
using DoctorLoan.Booking.Application.Features.Dtos;

namespace DoctorLoan.Booking.Application.Commons.Mappings;

public class MappingProfile : Profile, IOrderedMapperProfile
{
    public MappingProfile()
    {
        CreateMap<AddBookingCommand, Domain.Entities.Bookings.Booking>();
        CreateMap<Domain.Entities.Bookings.Booking, BookingDto>()
            .ForMember(s => s.FullName, otps => otps.MapFrom(fo => fo.Customer.FirstName.DisplayFullName(fo.Customer.LastName)))
            .ForMember(s => s.Phone, otps => otps.MapFrom(fo => "84".DisplayPhoneNo(fo.Customer.Phone)))
            .ForMember(s => s.Address, otps => otps.MapFrom(fo => fo.CustomerAddresses == null || fo.CustomerAddresses.Address == null ? string.Empty : fo.CustomerAddresses.Address.DisplayAddress()));
    }

    public int Order => 0;
}