using DoctorLoan.Application;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Domain.Entities.Products;
using DoctorLoan.Products.Application.Features.Categories.Dtos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.Products.Application.Features.Categories.Admin.Commands;
public class SaveCategoryCommandValidator : AbstractValidator<SaveCategoryCommand>
{
    public SaveCategoryCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
public class SaveCategoryCommand : CategoryDto, IRequest<Result<int>>
{
}
public class SaveCategoryCommandHandle : ApplicationBaseService<SaveCategoryCommandHandle>, IRequestHandler<SaveCategoryCommand, Result<int>>
{
    public SaveCategoryCommandHandle(ILogger<SaveCategoryCommandHandle> logger, IApplicationDbContext context, ICurrentRequestInfoService currentRequestInfoService, ICurrentTranslateService currentTranslateService, IDateTime dateTime) : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
    }

    public async Task<Result<int>> Handle(SaveCategoryCommand request, CancellationToken cancellationToken)
    {
        Category? category = new Category();
        if (request.Id > 0)
        {
            category = await _context.Categories.FindAsync(request.Id, cancellationToken);
            if (category == null)
            {
                return Result.Failed<int>(ServiceError.NotFound(_currentTranslateService));
            }

        }
        else
        {
            category.Slug = request.Name.ToSlug();
            
        }
        request.MapperTo(category);
        category.Status = category.Status > 0 ? category.Status : Domain.Enums.Commons.StatusEnum.Draft;
        if (category.Id == 0)
            _context.Categories.Add(category);

        if (string.IsNullOrEmpty(category.Code))
        {
            var maxCode = await _context.Categories.Select(s => s.Id).DefaultIfEmpty().MaxAsync(cancellationToken);
            category.Code = "C" + DateTime.Now.ToString("yy") + (maxCode + 1).ToString("D4");
        }

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success(category.Id);

    }
}
