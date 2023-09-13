using DoctorLoan.Application.Common.Mappings;
using DoctorLoan.Customer.Application.Features.Customers;
using DoctorLoan.Domain.Entities.Orders;
using DoctorLoan.Domain.Enums.Commons;
using DoctorLoan.Domain.Enums.Orders;
using Newtonsoft.Json;

namespace DoctorLoan.Order.Application.Features.Dtos;
public class OrderDto : IMapFrom<Domain.Entities.Orders.Order>
{
    public int Id { get; set; }
    public string OrderNo { get; set; }
    public int CustomerId { get; set; }
    public decimal SubTotal { get; set; }
    public decimal TotalPrice { get; set; }
    public OrderStatus Status { get; set; }
    public PaymentMethod PaymentMethod { get; set; }

    public string FullName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string AddressLine { get; set; }
    public string Remarks { get; set; }
    public DateTimeOffset? Created { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public List<OrderItemDto> OrderItems { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public CustomerDto Customer { get; set; }
}


public class OrderItemDto : IMapFrom<OrderItem>
{
    public int ProductItemId { get; set; }

    public string Name { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string? ProductSku { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string? OptionName { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public decimal TotalPrice { get; set; }
}

