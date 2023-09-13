using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.News.Application.Features.NewsCategories.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;
using FluentValidation;
using DoctorLoan.Domain.Entities.News;
using Microsoft.EntityFrameworkCore;
using DoctorLoan.Application.Common.Extentions;

namespace DoctorLoan.News.Application.Features.NewsCategories.Admin.Commands.SaveCategory;
public class SaveNewsCategoryCommandValidator : AbstractValidator<SaveNewsCategoryCommand>
{
    public SaveNewsCategoryCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
public class SaveNewsCategoryCommand:NewsCategoryDto,IRequest<Result<int>>
{
}
public class SaveNewsCategoryCommandHandle : ApplicationBaseService<SaveNewsCategoryCommandHandle>, IRequestHandler<SaveNewsCategoryCommand, Result<int>>
{
    public SaveNewsCategoryCommandHandle(ILogger<SaveNewsCategoryCommandHandle> logger, IApplicationDbContext context, ICurrentRequestInfoService currentRequestInfoService, ICurrentTranslateService currentTranslateService, IDateTime dateTime) : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
    }

    async Task<Result<int>> IRequestHandler<SaveNewsCategoryCommand, Result<int>>.Handle(SaveNewsCategoryCommand request, CancellationToken cancellationToken)
    {
        NewsCategory? category = null;
        if (request.Id > 0)
        {
            category = await _context.NewsCategories.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (category == null)
                return Result.Failed<int>(ServiceError.NotFound(_currentTranslateService));

        }
        else
        {
            category = new NewsCategory
            {
                Slug = request.Name.ToSlug(),
               
            };
        }
        request.MapperTo(category);
        category.Status = category.Status > 0 ? category.Status : Domain.Enums.Commons.StatusEnum.Draft;
        if (category.Id==0)
            _context.NewsCategories.Add(category);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success(category.Id);
        
    }
}

