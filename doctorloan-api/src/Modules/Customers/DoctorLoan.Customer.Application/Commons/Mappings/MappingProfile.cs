using AutoMapper;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Customer.Application.Features.CustomerAddresses;
using DoctorLoan.Customer.Application.Features.CustomerAddresses.Dtos;
using DoctorLoan.Customer.Application.Features.Customers;
using DoctorLoan.Customer.Application.Features.Customers.Dtos;
using DoctorLoan.Domain.Entities.Addresses;
using DoctorLoan.Domain.Entities.Bookings;
using DoctorLoan.Domain.Entities.Customers;

namespace DoctorLoan.Customer.Application.Commons.Mappings;
public class MappingProfile : Profile, IOrderedMapperProfile
{
    public MappingProfile()
    {
        CreateMap<AddCustomerCommand, Domain.Entities.Customers.Customer>()
            .ForMember(s => s.FullName, otps => otps.MapFrom(fo => fo.FirstName.DisplayFullName(fo.LastName)));

        CreateMap<AddCustomerAddressCommand, Address>();
        CreateMap<CustomerAddress, CustomerAddressDto>()
            .ForMember(s => s.Address, otps => otps.MapFrom(fo => fo.Address.DisplayAddress()))
            .ForMember(s => s.Phone, otps => otps.MapFrom(fo => "84".DisplayPhoneNo(fo.Phone)));

        CreateMap<Domain.Entities.Customers.Customer, CustomerDto>()
            .ForMember(s => s.Phone, otps => otps.MapFrom(fo => string.IsNullOrEmpty(fo.Phone) ? string.Empty : "0" + fo.Phone));

        CreateMap<Booking, CustomerBookingDto>()
            .ForMember(s => s.FullName, otps => otps.MapFrom(fo => fo.Customer.FullName))
            .ForMember(s => s.Phone, otps => otps.MapFrom(fo => "84".DisplayPhoneNo(fo.Customer.Phone)))
            .ForMember(s => s.Address, otps => otps.MapFrom(fo => fo.CustomerAddresses == null || fo.CustomerAddresses.Address == null ? string.Empty : fo.CustomerAddresses.Address.DisplayAddress()));
    }

    public int Order => 0;
}
