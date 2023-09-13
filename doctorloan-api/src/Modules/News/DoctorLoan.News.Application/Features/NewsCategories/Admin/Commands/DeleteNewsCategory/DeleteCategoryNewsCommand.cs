using DoctorLoan.Application;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Domain.Enums.Commons;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.News.Application.Features.NewsCategories.Admin.Commands;

public class DeleteCategoryNewsCommand :  IRequest<Result<int>>
{
    public int Id { get; set; }
}
public class DeleteCategoryNewsCommandHandle : ApplicationBaseService<DeleteCategoryNewsCommandHandle>, IRequestHandler<DeleteCategoryNewsCommand, Result<int>>
{
    public DeleteCategoryNewsCommandHandle(ILogger<DeleteCategoryNewsCommandHandle> logger, IApplicationDbContext context, ICurrentRequestInfoService currentRequestInfoService, ICurrentTranslateService currentTranslateService, IDateTime dateTime) : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
    }
    public async Task<Result<int>> Handle(DeleteCategoryNewsCommand request, CancellationToken cancellationToken)
    {
        var newsCategory = await _context.NewsCategories.FindAsync(request.Id, cancellationToken);
        if (newsCategory == null)
        {
            return Result.Failed<int>(ServiceError.NotFound(_currentTranslateService));
        }
        newsCategory.IsDeleted =true;     
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success(newsCategory.Id);

    }
}
