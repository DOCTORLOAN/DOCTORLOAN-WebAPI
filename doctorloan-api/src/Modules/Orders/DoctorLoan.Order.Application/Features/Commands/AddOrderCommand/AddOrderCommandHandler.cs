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
                var content = $"Cảm ơn bạn đã quan tâm sản phẩm DoctorLoan. <br/> Mã đơn hàng của bạn là: <b style=\"color: #d39364;\">{entity.OrderNo}</b> <br/>. Cảm ơn bạn!";

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
