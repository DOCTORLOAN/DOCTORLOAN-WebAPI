using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Application;
using DoctorLoan.News.Application.Features.News.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace DoctorLoan.News.Application.Features.News.Admin.Queries.GetTags;

public record GetTagsQuery() : IRequest<Result<List<TagDto>>>;
public class GetTagsQueryHandle : ApplicationBaseService<GetTagsQueryHandle>, IRequestHandler<GetTagsQuery, Result<List<TagDto>>>
{
    public GetTagsQueryHandle(ILogger<GetTagsQueryHandle> logger, IApplicationDbContext context, ICurrentRequestInfoService currentRequestInfoService, ICurrentTranslateService currentTranslateService, IDateTime dateTime) : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
    }

    public async Task<Result<List<TagDto>>> Handle(GetTagsQuery request, CancellationToken cancellationToken)
    {
        var tags = await _context.NewsTags.Select(x => new TagDto { Id = x.Id, Name = x.Name }).ToListAsync(cancellationToken);
        return Result.Success(tags);
    }
}
