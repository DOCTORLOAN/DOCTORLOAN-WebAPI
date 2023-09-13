using DoctorLoan.Application;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Domain.Entities.News;
using DoctorLoan.Domain.Enums.Commons;
using DoctorLoan.News.Application.Features.NewsCategories.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.News.Application.Features.NewsCategories.Admin.Queries;

public class  GetNewsCategoriesQuery:QueryParam, IRequest<Result<PaginatedList<NewsCategoryDto>>>
{
    public string? Slug { get; set; }
    public int? ParentId { get; set; }
    public StatusEnum? Status { get; set; }

}
public class GetNewsCategoriesQueryHandle : ApplicationBaseService<GetNewsCategoriesQueryHandle>, IRequestHandler<GetNewsCategoriesQuery, Result<PaginatedList<NewsCategoryDto>>>
{
    public GetNewsCategoriesQueryHandle(ILogger<GetNewsCategoriesQueryHandle> logger, IApplicationDbContext context, ICurrentRequestInfoService currentRequestInfoService, ICurrentTranslateService currentTranslateService, IDateTime dateTime) : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
    }
    public async Task<Result<PaginatedList<NewsCategoryDto>>> Handle(GetNewsCategoriesQuery request, CancellationToken cancellationToken)
    {
        var condition = PredicateBuilder.True<NewsCategory>();
        condition = condition.And(x => x.ParentId == request.ParentId);
        if (request.Slug != null)
            condition = condition.And(x => x.Slug == request.Slug);

     
        if (request.Status.HasValue)
            condition = condition.And(x => x.Status == request.Status);
        var query=_context.NewsCategories.Where(condition).OrderByDescending(c => c.Sort);
        var result =await Mapper.ProjectTo<NewsCategoryDto>(query).ToPagedListAsync(request.Page, request.Take);
        return Result.Success(result);
    }
}

