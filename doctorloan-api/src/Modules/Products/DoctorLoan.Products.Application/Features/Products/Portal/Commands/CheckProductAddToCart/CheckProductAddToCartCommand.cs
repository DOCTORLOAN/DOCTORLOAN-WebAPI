using DoctorLoan.Application;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.Products.Application.Features.Products.Portal.Commands;
public record CheckProductAddToCartCommand(int productId,int productItemId):IRequest<Result<int>>;
public class CheckProductAddToCartCommandHandler : ApplicationBaseService<CheckProductAddToCartCommandHandler>, IRequestHandler<CheckProductAddToCartCommand, Result<int>>
{
    public CheckProductAddToCartCommandHandler(ILogger<CheckProductAddToCartCommandHandler> logger, IApplicationDbContext context, ICurrentRequestInfoService currentRequestInfoService, ICurrentTranslateService currentTranslateService, IDateTime dateTime) : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
    }

    public async Task<Result<int>> Handle(CheckProductAddToCartCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.Include(x=>x.ProductItems)
            .FirstOrDefaultAsync(x =>x.Status==Domain.Enums.Commons.StatusEnum.Publish&& x.Id == request.productId
            &&(request.productItemId==0||x.ProductItems.Any(p=>p.Id==request.productItemId))
            
            );
        if (product == null)
            return Result.Failed<int>(ServiceError.NotFound(_currentTranslateService));
        if (product.ProductItems.Count > 1)
            return Result.Success(0);
        return Result.Success(product.ProductItems.First().Id);

    }
}
