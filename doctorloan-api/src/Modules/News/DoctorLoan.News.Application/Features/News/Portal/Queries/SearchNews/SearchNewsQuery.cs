using DoctorLoan.Application;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Domain.Entities.News;
using DoctorLoan.News.Application.Features.News.Portal.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.News.Application.Features.News.Portal.Queries.SearchNews;
public class SearchNewsQuery : QueryParam, IRequest<Result<PaginatedList<NewsSearchResultDto>>>
{
    public string? ViewName { get; set; }
    public List<int> CategoryIds { get; set; } = new List<int>();
    public string? CategorySlug { get; set; }
}

public class SearchNewsQueryHandle : ApplicationBaseService<SearchNewsQueryHandle>, IRequestHandler<SearchNewsQuery, Result<PaginatedList<NewsSearchResultDto>>>
{
    public SearchNewsQueryHandle(ILogger<SearchNewsQueryHandle> logger, IApplicationDbContext context, ICurrentRequestInfoService currentRequestInfoService, ICurrentTranslateService currentTranslateService, IDateTime dateTime) : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
    }

    public async Task<Result<PaginatedList<NewsSearchResultDto>>> Handle(SearchNewsQuery request, CancellationToken cancellationToken)
    {
        var condition = PredicateBuilder.True<NewsItem>();

        if (request.CategoryIds.Any())
            condition = condition.And(x => x.NewsCategories.Any(c => request.CategoryIds.Contains(c.NewsCategoryId)));
        if (request.CategorySlug != null)
            condition = condition.And(x => x.NewsCategories.Any(c => c.NewsCategory.Slug == request.CategorySlug));
        condition = condition.And(x => x.Status == Domain.Enums.Commons.StatusEnum.Publish);
        var query = _context.NewsItems.Where(condition)
                                        .OrderByDescending(x => x.Status)
                                            .ThenByDescending(s => s.LastModified);
        var result = await Mapper.ProjectTo<NewsSearchResultDto>(query).ToPagedListAsync(request.Page, request.Take);
        return Result.Success(result);
    }
}