using DoctorLoan.Application;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Domain.Enums.Commons;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.News.Application.Features.News.Admin.Commands;
public class UpdateNewsItemStatusCommand : IRequest<Result<bool>>
{
    public List<int> Ids { get; set; }=new List<int>();
    public StatusEnum Status { get; set; }
}
public class UpdateNewsItemStatusCommandHandle : ApplicationBaseService<UpdateNewsItemStatusCommandHandle>, IRequestHandler<UpdateNewsItemStatusCommand, Result<bool>>
{
    public UpdateNewsItemStatusCommandHandle(ILogger<UpdateNewsItemStatusCommandHandle> logger, IApplicationDbContext context, ICurrentRequestInfoService currentRequestInfoService, ICurrentTranslateService currentTranslateService, IDateTime dateTime) : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
    }

    public async Task<Result<bool>> Handle(UpdateNewsItemStatusCommand request, CancellationToken cancellationToken)
    {
        var listNew = await _context.NewsItems.Where(x => request.Ids.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach(var news in listNew) {
            news.Status = request.Status;
        }
        await _context.SaveChangesAsync(cancellationToken);
        return new Result<bool>(true);
    }
}
