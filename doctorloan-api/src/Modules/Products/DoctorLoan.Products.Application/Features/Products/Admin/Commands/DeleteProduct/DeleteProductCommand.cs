using DoctorLoan.Application;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Domain.Enums.Commons;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.Products.Application.Features.Products.Admin.Commands;
public class DeleteProductCommand : IRequest<Result<bool>>
{
    public int Id { get; set; }
}
public class DeleteProductCommandHandle : ApplicationBaseService<DeleteProductCommandHandle>, IRequestHandler<DeleteProductCommand, Result<bool>>
{
    public DeleteProductCommandHandle(ILogger<DeleteProductCommandHandle> logger, IApplicationDbContext context, ICurrentRequestInfoService currentRequestInfoService, ICurrentTranslateService currentTranslateService, IDateTime dateTime) 
        : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
    }

    public async Task<Result<bool>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FindAsync(request.Id, cancellationToken);
        if (product == null)
            return Result.Failed<bool>(ServiceError.NotFound(_currentTranslateService));
       product.IsDelete= true;
        await _context.SaveChangesAsync(cancellationToken);
        return new Result<bool>(true);
    }
}
