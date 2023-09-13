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
public class FilterCategoryQuery : QueryParam, IRequest<Result<PaginatedList<CategoryDto>>>
{
    public string? Keyword { get; set; }
}
public class FilterCategoryQueryHandle : ApplicationBaseService<FilterCategoryQueryHandle>, IRequestHandler<FilterCategoryQuery, Result<PaginatedList<CategoryDto>>>
{

    public FilterCategoryQueryHandle(ILogger<FilterCategoryQueryHandle> logger, IApplicationDbContext context, ICurrentRequestInfoService currentRequestInfoService, ICurrentTranslateService currentTranslateService, IDateTime dateTime) : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
    }

    public async Task<Result<PaginatedList<CategoryDto>>> Handle(FilterCategoryQuery request, CancellationToken cancellationToken)
    {
        var condition = PredicateBuilder.True<Category>();
        if (!string.IsNullOrEmpty(request.Keyword))
            condition = condition.And(x => x.Name.ToLower().Contains(request.Keyword));
        var query = _context.Categories.Where(condition)
                                            .OrderByDescending(x => x.Status)
                                                .ThenByDescending(x => x.LastModified);
        var result = await Mapper.ProjectTo<CategoryDto>(query).ToPagedListAsync(request.Page, request.Take);
        PreparingCategory(result.Items);

        return Result.Success(result);
    }
    private void PreparingCategory(List<CategoryDto> categories)
    {
        string GetName(string name, CategoryDto category)
        {
            if (category == null)
                return name;
            if (category.ParentId == 0)
                return name;
            var parent = categories.Find(x => x.Id == category.ParentId);
            return GetName(name, parent!) + " >> " + name;
        }
        foreach (var category in categories)
        {
            category.ParentName = GetName(category.Name, category);
        }
    }

}
