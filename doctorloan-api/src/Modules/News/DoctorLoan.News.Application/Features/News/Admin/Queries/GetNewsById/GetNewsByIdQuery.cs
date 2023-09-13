using DoctorLoan.Application;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Domain.Entities.News;
using DoctorLoan.News.Application.Features.News.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.News.Application.Features.News.Admin.Queries;
public record GetNewsItemByIdQuery(int id) : IRequest<Result<NewsItemDto>>;
public class GetNewsByIdQueryHandle : ApplicationBaseService<GetNewsByIdQueryHandle>, IRequestHandler<GetNewsItemByIdQuery, Result<NewsItemDto>>
{
    public GetNewsByIdQueryHandle(ILogger<GetNewsByIdQueryHandle> logger, IApplicationDbContext context, ICurrentRequestInfoService currentRequestInfoService, ICurrentTranslateService currentTranslateService, IDateTime dateTime) : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
    }

    public async Task<Result<NewsItemDto>> Handle(GetNewsItemByIdQuery request, CancellationToken cancellationToken)
    {
        var news=await _context.NewsItems
            .Include(x => x.NewsItemDetails)
                .Include(x=>x.NewsCategories)
                .Include(x=>x.NewsMedias).ThenInclude(x=>x.Media)
                .Include(x=>x.NewsTags).ThenInclude(x=>x.NewsTag)
            .FirstOrDefaultAsync(x=>x.Id==request.id, cancellationToken);
        if (news == null)
            return Result.Failed<NewsItemDto>(ServiceError.NotFound(_currentTranslateService));
        var model = news.MapperTo<NewsItem, NewsItemDto>();
        return Result.Success(model);
    }
}
