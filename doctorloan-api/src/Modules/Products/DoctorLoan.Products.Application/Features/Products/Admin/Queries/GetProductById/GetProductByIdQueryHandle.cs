using DoctorLoan.Application;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Domain.Entities.Products;
using DoctorLoan.Products.Application.Features.Products.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.Products.Application.Features.Products.Admin.Queries;
public record GetProductByIdQuery(int id) : IRequest<Result<ProductDto>>;
public class GetProductByIdQueryHandle : ApplicationBaseService<GetProductByIdQueryHandle>, IRequestHandler<GetProductByIdQuery, Result<ProductDto>>
{
    public GetProductByIdQueryHandle(ILogger<GetProductByIdQueryHandle> logger, IApplicationDbContext context, ICurrentRequestInfoService currentRequestInfoService, ICurrentTranslateService currentTranslateService, IDateTime dateTime) : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
    }

    public async Task<Result<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product=await _context.Products.Include(x => x.ProductItems).ThenInclude(x => x.ProductOptions)
                .Include(x=>x.ProductMedias).ThenInclude(x=>x.Media)
                .Include(x=>x.ProductDetails)
                .Include(x=>x.ProductAttributes)
                .Include(x=>x.ProductCategories)
            .FirstOrDefaultAsync(x=>x.Id==request.id, cancellationToken);
        if (product == null)
            return Result.Failed<ProductDto>(ServiceError.NotFound(_currentTranslateService));
        var model = product.MapperTo<Product, ProductDto>();
        return Result.Success(model);
    }
}
