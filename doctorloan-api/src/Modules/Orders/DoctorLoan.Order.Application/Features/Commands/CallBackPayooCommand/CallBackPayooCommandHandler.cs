using System.Security.Cryptography;
using System.Text;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Application;
using DoctorLoan.Domain.Enums.Commons;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace DoctorLoan.Order.Application.Features.Commands;
public class CallBackPayooCommandHandler : ApplicationBaseService<CallBackPayooCommandHandler>, IRequestHandler<CallBackPayooCommand, Result<bool>>
{
    public CallBackPayooCommandHandler(ILogger<CallBackPayooCommandHandler> logger, IApplicationDbContext context,
                                    ICurrentRequestInfoService currentRequestInfoService,
                                    ICurrentTranslateService currentTranslateService,
                                    IDateTime dateTime)
    : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
    }
    private readonly string _checkSumKey = "ZTJmMjk1NGU2OTNlODE0MjExZGM3MDM3MmJkNDI5NWU=";
    private readonly string _payooIpSan = "118.69.56.194";
    private readonly string _payooIpPro = "118.69.206.8";
    public async Task<Result<bool>> Handle(CallBackPayooCommand request, CancellationToken cancellationToken)
    {
        var _responseData = request.ResponseData;

        if (_responseData == null) return Result.Failed<bool>(ServiceError.NotFound(_currentTranslateService));

        if (_responseData.PaymentStatus != 1) return Result.Failed<bool>(ServiceError.NotFound(_currentTranslateService));

        var _hash = _checkSumKey + JsonConvert.SerializeObject(_responseData) + _payooIpSan;

        var sha512Hash = ComputeSha512Hash(_hash);
        bool isHashValid = sha512Hash.Equals(request.SecureHash, StringComparison.InvariantCultureIgnoreCase);

        if (!isHashValid) return Result.Failed<bool>(ServiceError.NotFound(_currentTranslateService));

        var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderNo == _responseData.OrderNo, cancellationToken);

        if (order is null) return Result.Failed<bool>(ServiceError.NotFound(_currentTranslateService));

        if (order.Status == OrderStatus.Completed && order.Status != OrderStatus.Return)
            return Result.Success(false);

        order.Status = OrderStatus.Confirm;
        order.Remarks = "Đã thanh toán Payoo mã đơn: " + _responseData.OrderNo;
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(true);
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