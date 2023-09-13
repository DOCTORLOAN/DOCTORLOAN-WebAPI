using DoctorLoan.Application;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.Customer.Application.Features.Customers;

public record DeleteCustomerCommand(int customerId) : IRequest<Result<bool>>;

public class DeleteUserCommandHandle : ApplicationBaseService<DeleteUserCommandHandle>, IRequestHandler<DeleteCustomerCommand, Result<bool>>
{

    public DeleteUserCommandHandle(ILogger<DeleteUserCommandHandle> logger,
    IApplicationDbContext context,
    ICurrentRequestInfoService currentRequestInfoService,
    ICurrentTranslateService currentTranslateService,
    IDateTime dateTime)
    : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    { }

    public async Task<Result<bool>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Customers.FindAsync(new object[] { request.customerId }, cancellationToken);
        if (entity == null) return Result.Failed<bool>(ServiceError.NotFound(_currentTranslateService));

        if (!entity.IsDelete)
        {
            entity.IsDelete = true;
            await _context.SaveChangesAsync(cancellationToken);
        }

        return Result.Success(true);
    }
}