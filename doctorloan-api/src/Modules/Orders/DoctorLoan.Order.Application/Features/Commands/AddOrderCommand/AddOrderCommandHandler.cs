using AutoMapper;
using BCrypt.Net;
using DoctorLoan.Application;
using DoctorLoan.Application.Features.Email;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Customer.Application.Commons.Expressions;
using DoctorLoan.Domain.Entities.Emails;
using DoctorLoan.Domain.Entities.Orders;
using DoctorLoan.Domain.Enums.Commons;
using DoctorLoan.Domain.Enums.Emails;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.Order.Application.Features.Commands;

public class AddOrderCommandHandler : ApplicationBaseService<AddOrderCommandHandler>, IRequestHandler<AddOrderCommand, Result<int>>
{
    private readonly IMapper _mapper;
    private readonly IEmailSenderService _emailSender;

    public AddOrderCommandHandler(ILogger<AddOrderCommandHandler> logger, IApplicationDbContext context,
                                    ICurrentRequestInfoService currentRequestInfoService,
                                    IMapper mapper,
                                    ICurrentTranslateService currentTranslateService,
                                    IEmailSenderService emailSender,
                                    IDateTime dateTime)
    : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
        _mapper = mapper;
        _emailSender = emailSender;
    }

    public async Task<Result<int>> Handle(AddOrderCommand request, CancellationToken cancellationToken)
    {
        var maxOrder = await _context.Orders.Select(s => s.Id).DefaultIfEmpty().MaxAsync(cancellationToken);

        var entity = _mapper.Map<Domain.Entities.Orders.Order>(request);

        entity.OrderNo = "ODL" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + (maxOrder + 1).ToString("D4");
        entity.Status = OrderStatus.Pending;

        var listProductId = request.ListItem.Select(s => s.ProductItemId);
        var listProductItem = _context.ProductItems.Where(s => listProductId.Contains(s.Id));
        var _itemProduct = "";
        var info = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
        foreach (var productItem in listProductItem)
        {
            var item = request.ListItem.FirstOrDefault(s => s.ProductItemId == productItem.Id);
            if (item == null) continue;

            var orderItem = new OrderItem()
            {
                ProductItemId = productItem.Id,
                Name = productItem.Name,
                Price = productItem.Price,
                Quantity = item.Quantity,
                TotalPrice = item.Quantity * productItem.Price,
            };

            _itemProduct += $"<tr>\r\n" +
                                $"<td style=\"color:#636363;border:1px solid #e5e5e5;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t" +
                                    $"{productItem.Name}\r\n" +
                                $"</td>\r\n" +
                                $"<td style=\"color:#636363;border:1px solid #e5e5e5;\">\r\n\t" +
                                    $"{item.Quantity}\r\n" +
                                $"</td>\r\n" +
                                $"<td style=\"color:#636363;border:1px solid #e5e5e5;\">\r\n\t" +
                                    $"Cái\r\n" +
                                $"</td>\r\n" +
                                $"<td style=\"color:#636363;border:1px solid #e5e5e5;\">\r\n\t" +
                                    $"<span>{String.Format(info, "{0:c}", productItem.Price)}</span>\r\n" +
                                $"</td>\r\n" +
                                $"<td style=\"color:#636363;border:1px solid #e5e5e5;\">\r\n\t" +
                                    $"<span>{String.Format(info, "{0:c}", (item.Quantity * productItem.Price))}</span>\r\n" +
                                $"</td>\r\n" +
                            $"</tr>\r\n\t\r\n";

            entity.OrderItems.Add(orderItem);
        }

        entity.TotalPrice = entity.OrderItems.Sum(s => s.TotalPrice);

        if (request.CustomerId.HasValue)
        {
            var exist = await _context.Customers.FindAsync(new object[] { request.CustomerId }, cancellationToken);
            if (exist is not null)
            {
                entity.CustomerId = exist.Id;
            }
        }

        if (entity.CustomerId <= 0)
        {
            var customerExistedPhone = await _context.Customers.FirstOrDefaultAsync(CustomerExpression.GetCustomerByPhone(request.Phone), cancellationToken);
            var customerExistedEmail = await _context.Customers.FirstOrDefaultAsync(CustomerExpression.GetCustomerByEmail(request.Email), cancellationToken);

            if (customerExistedEmail != null)
            {
                entity.CustomerId = customerExistedEmail.Id;
            }
            else
            {
                if (customerExistedPhone != null)
                    entity.CustomerId = customerExistedPhone.Id;
            }

            if (entity.CustomerId <= 0)
            {
                var salt = BCrypt.Net.BCrypt.GenerateSalt(10);
                var password = $"{request.Phone}@{DateTime.Now.Year}";
                var encryptPassword = BCrypt.Net.BCrypt.HashPassword(password, salt, false, HashType.SHA512);
                var newCustomer = new Domain.Entities.Customers.Customer()
                {
                    UID = Guid.NewGuid(),
                    PasswordHash = encryptPassword,
                    Phone = request.Phone,
                    Email = request.Email,
                    FullName = request.FullName,
                    FirstName = request.FullName,
                    Gender = Gender.Male
                };

                entity.Customer = newCustomer;
            }
        }

        await _context.Orders.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        try
        {
            if (!string.IsNullOrEmpty(request.Email))
            {
                var to = new List<ToInfo>() {
                    new ToInfo() { Mail = request.Email, Name = request.FullName }
            };
                
                var content = $"<table height=\"100%\" width=\"100%\" style=\"border:0;\">\r\n" +
                                    $"<tbody>\r\n\t" +
                                        $"<tr>\r\n" +
                                            $"<td align=\"center\" style=\"border: 0;\">\r\n\r\n\r\n\r\n" +
                                                $"<table border=\"0\" width=\"600\" >\r\n" +
                                                    $"<tbody>\r\n" +
                                                        $"<tr>\r\n" +
                                                            $"<td align=\"center\" style=\"border: 0;\">\r\n\t\t\t\t\t\r\n" +
                                                                $"<div align=\"center\">\r\n" +
                                                                    $"<h2>Phòng khám Đức Phúc DOCTORLOAN</h2>\r\n" +
                                                                    $"<p>ĐC: 9A, Tôn Thất Tùng, P.Phạm Ngũ Lão, Quận 1, TP.HCM</p>\r\n" +
                                                                    $"<p>ĐT: 0899136868</p>\r\n" +
                                                                    $"<h1 align=\"center\">Hoá đơn bán hàng</h1>\r\n" +
                                                                $"</div>\r\n\r\n" +
                                                                $"<table style=\"border: 0;\" width=\"600\">\r\n" +
                                                                    $"<tbody>\r\n\t" +
                                                                        $"<tr>\r\n\t" +
                                                                            $"<td style=\"border: 0;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\r\n" +
                                                                                $"<table style=\"border:0;\" width=\"100%\">\r\n" +
                                                                                    $"<tbody>\r\n" +
                                                                                        $"<tr>\r\n" +
                                                                                            $"<td style=\"padding:48px 48px 32px; border: 0;\">\r\n" +
                                                                                                $"<div style=\"color:#636363;font-size:14px;text-align:left\">\r\n\t\t\r\n" +
                                                                                                    $"<table style=\"width:100%;margin-bottom:40px;padding:0; border:0;\">\r\n" +
                                                                                                        $"<tbody>\r\n" +
                                                                                                            $"<tr>\r\n" +
                                                                                                                $"<td style=\"text-align:left; border:0;\">\r\n" +
                                                                                                                    $"Ngày: {DateTime.Now.ToString("d")}\r\n" +
                                                                                                                $"</td>\r\n" +
                                                                                                                $"<td style=\"text-align:right; border:0;\">\r\n" +
                                                                                                                    $"Số phiếu: {entity.OrderNo}\r\n" +
                                                                                                                $"</td>\r\n\r\n" +
                                                                                                            $"</tr>\r\n" +
                                                                                                            $"<tr>\r\n\t" +
                                                                                                                $"<td style=\"text-align:left; border:0;\">\r\n\t" +
                                                                                                                    $"Thu Ngân: Thiên Nga\r\n" +
                                                                                                                $"</td>\r\n" +
                                                                                                                $"<td style=\"text-align:right; border:0;\">\r\n\t" +
                                                                                                                    $"In lúc: {DateTime.Now.ToString("t")}\r\n" +
                                                                                                                $"</td>\r\n" +
                                                                                                            $"</tr>\r\n" +
                                                                                                            $"<tr>\r\n\t" +
                                                                                                                $"<td style=\"text-align:left; border:0;\" colspan=\"2\">\r\n\t" +
                                                                                                                    $"Khách hàng: {request.FullName}\r\n" +
                                                                                                                $"</td>\r\n" +
                                                                                                            $"</tr>\r\n" +
                                                                                                            $"<tr>\r\n\t" +
                                                                                                                $"<td style=\"text-align:left; border:0;\" colspan=\"2\">\r\n\t" +
                                                                                                                    $"Điện thoại: {request.Phone}\r\n" +
                                                                                                                $"</td>\r\n" +
                                                                                                            $"</tr>\r\n" +
                                                                                                            $"<tr>\r\n\t" +
                                                                                                                $"<td style=\"text-align:left; border:0;\" colspan=\"2\">\r\n\t" +
                                                                                                                    $"Địa chỉ: {request.AddressLine}\r\n" +
                                                                                                                $"</td>\r\n" +
                                                                                                            $"</tr>\r\n" +
                                                                                                            $"<tr>\r\n\t" +
                                                                                                                $"<td style=\"text-align:left; border:0;\" colspan=\"2\">\r\n\t" +
                                                                                                                    $"Ghi chú: {request.Remarks}\r\n" +
                                                                                                                $"</td>\r\n" +
                                                                                                            $"</tr>\r\n" +
                                                                                                        $"</tbody>\r\n" +
                                                                                                    $"</table>\r\n\r\n\r\n" +
                                                                                                $"<div style=\"margin-bottom:40px\">\r\n" +
                                                                                                $"<table cellspacing=\"0\" cellpadding=\"6\" width=\"100%\" style=\"color:#636363;border:0 ;\">\r\n" +
                                                                                                    $"<thead>\r\n" +
                                                                                                        $"<tr>\r\n" +
                                                                                                            $"<th scope=\"col\" style=\"color:#636363;border:1px solid #e5e5e5;padding:12px;\">Mặt hàng</th>\r\n" +
                                                                                                            $"<th scope=\"col\" style=\"color:#636363;border:1px solid #e5e5e5;padding:12px;\">SL</th>\r\n" +
                                                                                                            $"<th scope=\"col\" style=\"color:#636363;border:1px solid #e5e5e5;padding:12px;\">ĐVT</th>\r\n" +
                                                                                                            $"<th scope=\"col\" style=\"color:#636363;border:1px solid #e5e5e5;padding:12px;\">Gía</th>\r\n" +
                                                                                                            $"<th scope=\"col\" style=\"color:#636363;border:1px solid #e5e5e5;padding:12px;\">T tiền</th>\r\n" +
                                                                                                        $"</tr>\r\n" +
                                                                                                    $"</thead>\r\n" +
                                                                                                    $"<tbody>\r\n" +
                                                                                                        $"{_itemProduct}" + 
                                                                                                    $"</tbody>\r\n" +
                                                                                                    $"<tfoot>\r\n" +
                                                                                                        $"<tr>\r\n" +
                                                                                                            $"<th colspan=\"4\" style=\"border:0;\">\r\n\tTổng SL:\r\n</th>\r\n" +
                                                                                                            $"<td colspan=\"2\" style=\"border: 0;\">\r\n\t" +
                                                                                                                $"1\r\n" +
                                                                                                            $"</td>\r\n" +
                                                                                                        $"</tr>\r\n" +
                                                                                                        $"<tr>\r\n<th colspan=\"4\" style=\"border: 0;\">\r\n\tTiền hàng:\r\n</th>\r\n" +
                                                                                                            $"<td style=\"border: 0;\" colspan=\"2\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t" +
                                                                                                                $"<span>{String.Format(info, "{0:c}", entity.TotalPrice)}</span>\r\n" +
                                                                                                            $"</td>\r\n" +
                                                                                                        $"</tr>\r\n" +
                                                                                                        $"<tr>\r\n" +
                                                                                                            $"<th colspan=\"4\" style=\"border:0;\">\r\n\tKhuyễn mãi:\r\n</th>\r\n" +
                                                                                                            $"<td colspan=\"2\" style=\"border:0;\">\r\n\t\r\n</td>\r\n" +
                                                                                                        $"</tr>\r\n" +
                                                                                                        $"<tr>\r\n<th colspan=\"4\" style=\"border:0;\">\r\n\tTổng:\r\n</th>\r\n" +
                                                                                                            $"<td style=\"border:0;\" colspan=\"2\">\r\n\t" +
                                                                                                                $"<span>{String.Format(info, "{0:c}", entity.TotalPrice)}</span>\r\n" +
                                                                                                            $"</td>\r\n" +
                                                                                                        $"</tr>\r\n" +
                                                                                                        $"<tr>\r\n<th colspan=\"4\" style=\"border:0;\">\r\n\tChuyển khoản:\r\n</th>\r\n" +
                                                                                                            $"<td colspan=\"2\" style=\"border:0;\">\r\n\t" +
                                                                                                                $"<span>{String.Format(info, "{0:c}", entity.TotalPrice)}</span>\r\n" +
                                                                                                            $"</td>\r\n" +
                                                                                                        $"</tr>\r\n" +
                                                                                                    $"</tfoot>\r\n" +
                                                                                                $"</table>\r\n" +
                                                                                            $"</div>\r\n\r\n</div>\r\n\r\n" +
                                                                                            $"<div align=\"center\">\r\n" +
                                                                                                $"<a href=\"https://doctorloan.vn/\">\r\ndoctorloan.vn</a>\r\n" +
                                                                                                $"<p>Đây là email động được tạo từ danh sách đăng ký của chúng tôi. Do đó, xin đừng trả lời email này.</p>\r\n" +
                                                                                            $"</div>\r\n\r\n" +
                                                                                        $"</td>\r\n" +
                                                                                    $"</tr>\r\n" +
                                                                                $"</tbody>\r\n" +
                                                                            $"</table>\r\n\t\t\t\t\t\t\t\t\t\r\n" +
                                                                        $"</td>\r\n" +
                                                                    $"</tr>\r\n" +
                                                                $"</tbody>\r\n" +
                                                            $"</table>\r\n\t\t\t\t\t\t\t\t\t\r\n" +
                                                        $"</td>\r\n" +
                                                    $"</tr>\r\n" +
                                                $"</tbody>\r\n" +
                                            $"</table>\r\n" +
                                        $"</td>\r\n" +
                                    $"</tr>\r\n\r\n" +
                                $"</tbody>\r\n" +
                            $"</table>";

                var message = new MessageEmail(to, "[DOCTORLOAN] Đơn hàng", content);
                var logRequest = new EmailRequest
                {
                    Code = content,
                    Email = string.Join(",", to),
                    Type = EmailType.None
                };

                _ = await _emailSender.SendEmail(message, logRequest, cancellationToken);
            }
        }
        catch
        {
            _logger.LogError($"Send email order error: {entity.Id}");
        }

        return Result.Success(entity.Id);
    }
}
