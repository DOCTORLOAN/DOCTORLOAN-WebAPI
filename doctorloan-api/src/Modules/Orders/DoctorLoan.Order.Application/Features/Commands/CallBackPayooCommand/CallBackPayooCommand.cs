using DoctorLoan.Application.Models.Commons;
using MediatR;

namespace DoctorLoan.Order.Application.Features.Commands.CallBackPayooCommand;

//public class ResponseData
//{
//    public string? PaymentMethod { get; set; }

//    public string? PurchaseDate { get; set; }

//    public string? MerchantUsername { get; set; }

//    public int? ShopId { get; set; }

//    public int? MasterShopId { get; set; }

//    public string? OrderNo { get; set; }

//    public decimal? OrderCash { get; set; }

//    public string? BankName { get; set; }

//    public string? CardNumber { get; set; }

//    public int? CardIssuanceType { get; set; }

//    public int? PaymentStatus { get; set; }

//    public string? AuthorizationNo { get; set; }

//    public string? PaymentSource { get; set; }

//    public decimal? VoucherTotalAmount { get; set; }

//    public string? PYTransId { get; set; }

//    public string? PaymentMethodName { get; set; }
//}

//public class CallBackPayooCommand : IRequest<Result<bool>>
//{
//    public string? ResponseData { get; set; }

//    public string Signature { get; set; }

//    public string? SecureHash { get; set; }
//}

public class CallBackPayooCommand : IRequest<Result<bool>>
{
    public ResponseData ResponseData { get; set; }
    public string Signature { get; set; }
    public string SecureHash { get; set; }
}

public class ResponseData
{
    public string PaymentMethod { get; set; }
    public string PurchaseDate { get; set; }
    public string MerchantUsername { get; set; }
    public int ShopId { get; set; }
    public int MasterShopId { get; set; }
    public string? OrderNo { get; set; }
    public decimal OrderCash { get; set; }
    public string BankName { get; set; }
    public string CardNumber { get; set; }
    public int CardIssuanceType { get; set; }
    public int PaymentStatus { get; set; }
    public string AuthorizationNo { get; set; }
    public string PaymentSource { get; set; }
    public decimal VoucherTotalAmount { get; set; }
    public string PyTransId { get; set; }
    public string PaymentMethodName { get; set; }
}

