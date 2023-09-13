using DoctorLoan.Application;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Domain.Enums.Commons;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.News.Application.Features.NewsCategories.Admin.Commands;

public class UpdateCategoryNewsStatusCommand :  IRequest<Result<int>>
{
    public int Id { get; set; }
    public StatusEnum Status { get; set; }
}
public class UpdateCategoryNewsStatusCommandHandle : ApplicationBaseService<UpdateCategoryNewsStatusCommandHandle>, IRequestHandler<UpdateCategoryNewsStatusCommand, Result<int>>
{
    public UpdateCategoryNewsStatusCommandHandle(ILogger<UpdateCategoryNewsStatusCommandHandle> logger, IApplicationDbContext context, ICurrentRequestInfoService currentRequestInfoService, ICurrentTranslateService currentTranslateService, IDateTime dateTime) : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
    }
    public async Task<Result<int>> Handle(UpdateCategoryNewsStatusCommand request, CancellationToken cancellationToken)
    {
        var newsCategory = await _context.Categories.FindAsync(request.Id, cancellationToken);
        if (newsCategory == null)
        {
            return Result.Failed<int>(ServiceError.NotFound(_currentTranslateService));
        }
        newsCategory.Status =request.Status;     
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success(newsCategory.Id);

    }
}
