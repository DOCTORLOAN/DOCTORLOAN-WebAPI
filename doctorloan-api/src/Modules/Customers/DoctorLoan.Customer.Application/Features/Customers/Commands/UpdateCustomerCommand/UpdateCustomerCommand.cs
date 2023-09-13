using DoctorLoan.Application;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Customer.Application.Commons.Expressions;
using DoctorLoan.Domain.Enums.Commons;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.Customer.Application.Features.Customers;
public class UpdateCustomerCommand : IRequest<Result<int>>
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    private string _phone;
    public string Phone
    {
        get => _phone.RemoveFirstZero();
        set { _phone = value; }
    }
    public Gender Gender { get; set; }

    public string Email { get; set; }
    public DateTimeOffset? DOB { get; set; }
}


public class UpdateCustomerCommandHandler : ApplicationBaseService<UpdateCustomerCommandHandler>, IRequestHandler<UpdateCustomerCommand, Result<int>>
{
    public UpdateCustomerCommandHandler(ILogger<UpdateCustomerCommandHandler> logger, IApplicationDbContext context,
                                    ICurrentRequestInfoService currentRequestInfoService,
                                    ICurrentTranslateService currentTranslateService,
                                    IDateTime dateTime)
    : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {

    }

    public async Task<Result<int>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers.FindAsync(new object[] { request.Id }, cancellationToken);
        if (customer is null)
        {
            return Result.Failed<int>(ServiceError.NotFound(_currentTranslateService));
        }

        if (!string.IsNullOrEmpty(request.Email) && customer.Email?.Trim().ToLower() != request.Email.Trim().ToLower())
        {
            var customerExistedEmail = await _context.Customers.FirstOrDefaultAsync(CustomerExpression.GetCustomerByEmail(request.Email), cancellationToken);
            if (customerExistedEmail != null)
            {
                return Result.Failed<int>("Email đã được sử dụng tài khoản khác.");
            }
        }

        if (!string.IsNullOrEmpty(request.Phone) && customer.Phone?.Trim() != request.Phone.Trim())
        {
            var customerExisted = await _context.Customers.FirstOrDefaultAsync(CustomerExpression.GetCustomerByPhone(request.Phone), cancellationToken);
            if (customerExisted != null)
            {
                return Result.Failed<int>("Số điện thoại đã được sử dụng tài khoản khác.");
            }
        }

        customer.FirstName = request.FirstName;
        customer.LastName = request.LastName;
        customer.FullName = request.FirstName.DisplayFullName(request.LastName);
        customer.DOB = request.DOB;
        customer.Email = request.Email;
        customer.Phone = request.Phone;
        customer.Gender = request.Gender;

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success(customer.Id);
    }
}