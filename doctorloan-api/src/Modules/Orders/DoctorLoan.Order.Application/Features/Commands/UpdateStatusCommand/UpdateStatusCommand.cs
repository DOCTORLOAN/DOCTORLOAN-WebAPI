using DoctorLoan.Application;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Domain.Enums.Commons;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.Order.Application.Features.Commands;

public class UpdateStatusCommand : IRequest<Result<bool>>
{
    public int Id { get; set; }
    public OrderStatus Status { get; set; }
    public string? Remarks { get; set; }
}

public class UpdateStatusCommandHandler : ApplicationBaseService<UpdateStatusCommandHandler>, IRequestHandler<UpdateStatusCommand, Result<bool>>
{
    public UpdateStatusCommandHandler(ILogger<UpdateStatusCommandHandler> logger, IApplicationDbContext context,
                                    ICurrentRequestInfoService currentRequestInfoService,
                                    ICurrentTranslateService currentTranslateService,
                                    IDateTime dateTime)
    : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
    }

    public async Task<Result<bool>> Handle(UpdateStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders.FindAsync(new object[] { request.Id }, cancellationToken);
        if (order is null) return Result.Failed<bool>(ServiceError.NotFound(_currentTranslateService));

        if (order.Status == OrderStatus.Completed && request.Status != OrderStatus.Return)
            return Result.Success(false);

        if (request.Status <= order.Status)
            return Result.Success(false);

        order.Status = request.Status;
        order.Remarks = request.Remarks;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(true);
    }
}
