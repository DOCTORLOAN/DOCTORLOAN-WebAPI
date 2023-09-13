using AutoMapper;
using DoctorLoan.Application;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Domain.Entities.News;
using DoctorLoan.Domain.Enums.Commons;
using DoctorLoan.News.Application.Features.News.Admin.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.News.Application.Features.News.Admin.Queries;
public class FilterNewsItemsQuery : QueryParam, IRequest<Result<PaginatedList<NewsItemFilterResultDto>>>
{
    public string? Keyword { get; set; }
    public StatusEnum? Status { get; set; }
    public int? CategoryId { get; set; }
}
public class FilterProductQueryHandle : ApplicationBaseService<FilterProductQueryHandle>, IRequestHandler<FilterNewsItemsQuery, Result<PaginatedList<NewsItemFilterResultDto>>>
{
    private readonly IMapper _mapper;
    public FilterProductQueryHandle(IMapper mapper, ILogger<FilterProductQueryHandle> logger, IApplicationDbContext context, ICurrentRequestInfoService currentRequestInfoService, ICurrentTranslateService currentTranslateService, IDateTime dateTime) : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
        _mapper = mapper;
    }

    public async Task<Result<PaginatedList<NewsItemFilterResultDto>>> Handle(FilterNewsItemsQuery request, CancellationToken cancellationToken)
    {
        var condition = PredicateBuilder.True<NewsItem>();
        if (request.Keyword != null)
            condition = condition.And(x => x.Title.ToLower().Contains(request.Keyword.ToLower()));
        if (request.Status.HasValue)
        {
            condition = condition.And(x => x.Status == request.Status);
        }
        if (request.CategoryId.HasValue)
            condition = condition.And(x => x.NewsCategories.Any(c => c.NewsCategoryId == request.CategoryId));
        var query = _context.NewsItems.Where(condition)
                                        .OrderByDescending(x => x.Status)
                                            .ThenByDescending(s => s.LastModified);

        var data = await _mapper.ProjectTo<NewsItemFilterResultDto>(query)
            .ToPagedListAsync(request.Page, request.Take, cancellationToken);
        return Result.Success(data);
    }
}