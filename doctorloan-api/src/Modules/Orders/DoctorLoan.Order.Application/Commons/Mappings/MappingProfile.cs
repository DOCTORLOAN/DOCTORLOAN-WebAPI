using AutoMapper;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Domain.Entities.Orders;
using DoctorLoan.Order.Application.Features.Commands;
using DoctorLoan.Order.Application.Features.Dtos;

namespace DoctorLoan.Order.Application.Commons.Mappings;

public class MappingProfile : Profile, IOrderedMapperProfile
{
    public MappingProfile()
    {
        CreateMap<AddOrderCommand, Domain.Entities.Orders.Order>();
        CreateMap<Domain.Entities.Orders.Order, OrderDto>()
            .ForMember(s => s.Phone, otps => otps.MapFrom(fo => "84".DisplayPhoneNo(fo.Customer.Phone)));


        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(s => s.ProductSku, otps => otps.MapFrom(fo => fo.ProductItem != null ? fo.ProductItem.Sku : string.Empty))
            .ForMember(s => s.OptionName, otps => otps.MapFrom(fo => fo.ProductItem != null && fo.ProductItem.ProductOptions.Any()
                                                                        ? string.Join(' ', fo.ProductItem.ProductOptions.Select(s => s.Name).ToList())
                                                                        : string.Empty));
    }

    public int Order => 0;
}
