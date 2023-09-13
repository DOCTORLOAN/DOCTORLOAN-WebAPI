using DoctorLoan.Application;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.Customer.Application.Features.CustomerAddresses;


public record DeleteCustomerAddressCommand(int Id) : IRequest<Result<bool>>;

public class DeleteCustomerAddressCommandHandle : ApplicationBaseService<DeleteCustomerAddressCommandHandle>, IRequestHandler<DeleteCustomerAddressCommand, Result<bool>>
{

    public DeleteCustomerAddressCommandHandle(ILogger<DeleteCustomerAddressCommandHandle> logger,
    IApplicationDbContext context,
    ICurrentRequestInfoService currentRequestInfoService,
    ICurrentTranslateService currentTranslateService,
    IDateTime dateTime)
    : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    { }

    public async Task<Result<bool>> Handle(DeleteCustomerAddressCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.CustomerAddresses.FindAsync(new object[] { request.Id }, cancellationToken);
        if (entity == null) return Result.Failed<bool>(ServiceError.NotFound(_currentTranslateService));

        entity.IsDelete = true;
        if (entity.IsDefault)
        {
            var firstCustAddress = _context.CustomerAddresses.FirstOrDefault(s => s.CustomerId == entity.CustomerId);
            if (firstCustAddress is not null)
            {
                firstCustAddress.IsDefault = true;
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(true);
    }
}