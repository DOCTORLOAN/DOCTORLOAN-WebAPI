using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Domain.Enums.Orders;
using DoctorLoan.Order.Application.Features.Dtos;
using MediatR;

namespace DoctorLoan.Order.Application.Features.Commands;

public class AddOrderCommand : IRequest<Result<int>>
{
    public int? CustomerId { get; set; }
    public decimal SubTotal { get; set; }
    public decimal TotalPrice { get; set; }
    public PaymentMethod PaymentMethod { get; set; }

    public string FullName { get; set; }
    private string _phone;
    public string Phone
    {
        get => _phone.RemoveFirstZero();
        set { _phone = value; }
    }
    public string Email { get; set; }
    public string AddressLine { get; set; }
    public string Remarks { get; set; }

    public List<OrderItemDto> ListItem { get; set; }
}
