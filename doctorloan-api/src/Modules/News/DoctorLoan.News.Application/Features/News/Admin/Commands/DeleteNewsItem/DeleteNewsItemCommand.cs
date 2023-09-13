using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Application;
using DoctorLoan.Domain.Enums.Commons;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.News.Application.Features.News.Admin.Commands.DeleteNewsItem;

public class DeleteNewsItemCommand : IRequest<Result<bool>>
{
    public int Id { get; set; }
}
public class DeleteNewsItemCommandHandle : ApplicationBaseService<DeleteNewsItemCommandHandle>, IRequestHandler<DeleteNewsItemCommand, Result<bool>>
{
    public DeleteNewsItemCommandHandle(ILogger<DeleteNewsItemCommandHandle> logger, IApplicationDbContext context, ICurrentRequestInfoService currentRequestInfoService, ICurrentTranslateService currentTranslateService, IDateTime dateTime) : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
    }

    public async Task<Result<bool>> Handle(DeleteNewsItemCommand request, CancellationToken cancellationToken)
    {
        var newsItem = await _context.NewsItems.FindAsync(request.Id,cancellationToken);
        if (newsItem == null)
            return Result.Failed<bool>(ServiceError.NotFound(_currentTranslateService));
        newsItem.IsDeleted= true;
        await _context.SaveChangesAsync(cancellationToken);
        return new Result<bool>(true);
    }
}