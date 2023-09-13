using DoctorLoan.Application;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.News.Application.Features.NewsCategories.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.News.Application.Features.NewsCategories.Admin.Queries;

public record class GetNewsCategoryByIdQuery(int id):IRequest<Result<NewsCategoryDto>>;
public class GetCategoryByIdQueryHandle : ApplicationBaseService<GetCategoryByIdQueryHandle>, IRequestHandler<GetNewsCategoryByIdQuery, Result<NewsCategoryDto>>
{
    public GetCategoryByIdQueryHandle(ILogger<GetCategoryByIdQueryHandle> logger, IApplicationDbContext context, ICurrentRequestInfoService currentRequestInfoService, ICurrentTranslateService currentTranslateService, IDateTime dateTime) : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
    }
    public async Task<Result<NewsCategoryDto>> Handle(GetNewsCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category=await Mapper.ProjectTo<NewsCategoryDto>(_context.NewsCategories.Where(x=>x.Id==request.id)).FirstOrDefaultAsync(cancellationToken);
        if (category == null)
            return Result.Failed<NewsCategoryDto>(ServiceError.NotFound(_currentTranslateService));
        return Result.Success(category);
    }
}

