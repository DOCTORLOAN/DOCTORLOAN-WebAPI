using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Application;
using DoctorLoan.Domain.Enums.Commons;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Org.BouncyCastle.Asn1.Ocsp;

namespace DoctorLoan.Order.Application.Features.Commands.CallBackPayooCommand;
public class CallBackPayooCommandHandler : ApplicationBaseService<CallBackPayooCommandHandler>, IRequestHandler<CallBackPayooCommand, Result<bool>>
{
    public CallBackPayooCommandHandler(ILogger<CallBackPayooCommandHandler> logger, IApplicationDbContext context,
                                    ICurrentRequestInfoService currentRequestInfoService,
                                    ICurrentTranslateService currentTranslateService,
                                    IDateTime dateTime)
    : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
    }
    private readonly string _checkSumKey = "ZTJmMjk1NGU2OTNlODE0MjExZGM3MDM3MmJkNDI5NWU="; // Replace with your actual key
    private readonly string _payooIpSan = "118.69.56.194"; // Replace with your actual IP SAN
    private readonly string _payooIpPro = "118.69.206.8"; // Replace with your actual IP PRO
    public async Task<Result<bool>> Handle(CallBackPayooCommand request, CancellationToken cancellationToken)
    {
        var _responseData = request.ResponseData;

        if (_responseData == null) return Result.Failed<bool>(ServiceError.NotFound(_currentTranslateService));

        if (_responseData.PaymentStatus != 1) return Result.Failed<bool>(ServiceError.NotFound(_currentTranslateService));

        var tmp = JsonConvert.SerializeObject(request.ResponseData);

        string _hash = _checkSumKey + tmp + _payooIpSan;
        //string _hash = "";
        //var _hash = "ZTJmMjk1NGU2OTNlODE0MjExZGM3MDM3MmJkNDI5NWU={\"PaymentMethod\":\"2\",\"PurchaseDate\":\"20240516160417\",\"MerchantUsername\":\"SB_DOCTORLOAN\",\"ShopId\":11977,\"MasterShopId\":11977,\"OrderNo\":\"ORD_11977_ODL24050016\",\"OrderCash\":273000000.00000000,\"BankName\":\"VISA\",\"CardNumber\":\"41111111****1111\",\"CardIssuanceType\":0,\"PaymentStatus\":1,\"AuthorizationNo\":\"160417\",\"PaymentSource\":\"VISA\",\"VoucherTotalAmount\":0.00,\"PYTransId\":\"ORD_11977_ODL24050016_11977\",\"PaymentMethodName\":\"cc\"}118.69.56.194";

        var sha512Hash = ComputeSha512Hash(_hash);
        //bool isHashValid = sha512Hash.Equals(request.SecureHash, StringComparison.InvariantCultureIgnoreCase);
        bool isHashValid = sha512Hash.Equals("0DBCD859201DF3827F2607D7BBB2691B2961B276A1E84D909178C32FF91AF0B5176BD8178306BEE77C860C7F75728F382D08C207A4948C39DBC7B29648820D05", StringComparison.OrdinalIgnoreCase);

        if (!isHashValid) return Result.Failed<bool>(ServiceError.NotFound(_currentTranslateService));

        var order = await _context.Orders.FindAsync(new object[] { _responseData.OrderNo }, cancellationToken);
        if (order is null) return Result.Failed<bool>(ServiceError.NotFound(_currentTranslateService));

        if (order.Status == OrderStatus.Completed && order.Status != OrderStatus.Return &&
            order.StatusPayment == OrderStatusPayment.Payment && order.StatusPayment == OrderStatusPayment.Cancel)
            return Result.Success(false);

        order.Status = OrderStatus.Confirm;
        order.StatusPayment = OrderStatusPayment.Payment;
        order.Remarks = "Đã thanh toán Payoo mã đơn: " + _responseData.OrderNo;
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(true);

        //var paymentInfo = new PaymentInfo
        //{
        //    ReturnCode = 1,
        //    Description = "NOTIFY_RECEIVED"
        //};
        //return Result.Success(paymentInfo);
    }

    private string ComputeSha512Hash(string rawData)
    {
        var message = Encoding.UTF8.GetBytes(rawData);
        using (var alg = SHA512.Create())
        {
            string hex = "";

            var hashValue = alg.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }
    }
}
