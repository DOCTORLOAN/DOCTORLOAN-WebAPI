﻿using DoctorLoan.Application;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Domain.Enums.Commons;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.Products.Application.Features.Categories.Admin.Commands;

public class DeleteCategoryCommand :  IRequest<Result<int>>
{
    public int Id { get; set; }
    public StatusEnum Status { get; set; }
}
public class DeleteCategoryCommandCommandHandle : ApplicationBaseService<DeleteCategoryCommandCommandHandle>, IRequestHandler<DeleteCategoryCommand, Result<int>>
{
    public DeleteCategoryCommandCommandHandle(ILogger<DeleteCategoryCommandCommandHandle> logger, IApplicationDbContext context, ICurrentRequestInfoService currentRequestInfoService, ICurrentTranslateService currentTranslateService, IDateTime dateTime) : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
    }
    public async Task<Result<int>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories.FindAsync(request.Id, cancellationToken);
        if (category == null)
        {
            return Result.Failed<int>(ServiceError.NotFound(_currentTranslateService));
        }
        category.IsDeleted =true;     
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success(category.Id);

    }
}
