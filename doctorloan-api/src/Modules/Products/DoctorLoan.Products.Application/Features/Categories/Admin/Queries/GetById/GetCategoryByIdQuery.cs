using DoctorLoan.Application;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Domain.Entities.Products;
using DoctorLoan.Products.Application.Features.Categories.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.Products.Application.Features.Categories.Admin.Queries;
public record GetCategoryByIdQuery(int id):IRequest<Result<CategoryDto>>
{
}
public class GetCategoryByIdQueryHandle : ApplicationBaseService<GetCategoryByIdQueryHandle>, IRequestHandler<GetCategoryByIdQuery, Result<CategoryDto>>
{
    public GetCategoryByIdQueryHandle(ILogger<GetCategoryByIdQueryHandle> logger, IApplicationDbContext context, ICurrentRequestInfoService currentRequestInfoService, ICurrentTranslateService currentTranslateService, IDateTime dateTime) : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
    }

    public async Task<Result<CategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories.FindAsync(request.id, cancellationToken);
        if (category == null)
        {
            return Result.Failed<CategoryDto>(ServiceError.NotFound(_currentTranslateService));
        }
        return Result.Success(category.MapperTo<Category, CategoryDto>());
    }
}
