using DoctorLoan.Application;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Domain.Entities.Products;
using DoctorLoan.Products.Application.Features.Products.Portal.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.Products.Application.Features.Products.Portal.Queries;
public class SearchProductsQuery : QueryParam, IRequest<Result<PaginatedList<SearchProductResultDto>>>
{
    public string? ViewName { get; set; }
    public List<int> CategoryIds { get; set; } = new List<int>();
    public string? CategoryCode { get; set; }
    public string? Keyword { get; set; }
    public int IgnoreId { get; set; }
}
public class SearchProductsQueryHandle : ApplicationBaseService<SearchProductsQueryHandle>, IRequestHandler<SearchProductsQuery, Result<PaginatedList<SearchProductResultDto>>>
{
    public SearchProductsQueryHandle(ILogger<SearchProductsQueryHandle> logger, IApplicationDbContext context, ICurrentRequestInfoService currentRequestInfoService, ICurrentTranslateService currentTranslateService, IDateTime dateTime) : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
    }

    public async Task<Result<PaginatedList<SearchProductResultDto>>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
    {
        var condition = PredicateBuilder.True<Product>();
        if (!string.IsNullOrEmpty(request.Keyword))
            condition = condition.And(x => x.Name.ToLower().Contains(request.Keyword) || x.Sku.Contains(request.Keyword) || x.Slug.Contains(request.Keyword));
        if (request.CategoryIds.Any())
            condition = condition.And(x => x.ProductCategories.Any(c => request.CategoryIds.Contains(c.CategoryId)));
        if (request.CategoryCode != null)
            condition = condition.And(x => x.ProductCategories.Any(c => c.Category.Code == request.CategoryCode));
        if (request.IgnoreId > 0)
            condition = condition.And(x => x.Id != request.IgnoreId);

        condition = condition.And(x => x.Status == Domain.Enums.Commons.StatusEnum.Publish);
        var query = _context.Products.Where(condition)
                                     .OrderByDescending(x => x.Status)
                                         .ThenByDescending(s => s.LastModified); ;
        var result = await Mapper.ProjectTo<SearchProductResultDto>(query).ToPagedListAsync(request.Page, request.Take);
        return Result.Success(result);
    }
}
