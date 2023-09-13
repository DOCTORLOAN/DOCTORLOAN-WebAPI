using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application;
using DoctorLoan.Application.Models.Commons;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using DoctorLoan.Products.Application.Features.Products.Dtos;

namespace DoctorLoan.Products.Application.Features.Products.Admin.Queries;

public record GetOptionsQuery : IRequest<Result<ListProductOptions>>;

public class GetDataSelectQueryHandle : ApplicationBaseService<GetDataSelectQueryHandle>, IRequestHandler<GetOptionsQuery, Result<ListProductOptions>>
{
    public GetDataSelectQueryHandle(ILogger<GetDataSelectQueryHandle> logger,
                             IApplicationDbContext context,
                             ICurrentRequestInfoService currentRequestInfoService,
                             ICurrentTranslateService currentTranslateService,
                             IDateTime dateTime)
    : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
    }

    public async Task<Result<ListProductOptions>> Handle(GetOptionsQuery request, CancellationToken cancellationToken)
    {
        var data = new ListProductOptions
        {
            Attributes=await _context.Attributes.Select(x=>new Option { Value=x.Id,Label=x.Name}).ToListAsync(cancellationToken),
            Categories = await _context.Categories.Select(x => new Option { Value = x.Id, Label = x.Name }).ToListAsync(cancellationToken),
            OptionGroups = await _context.ProductOptionGroups.Select(x => new Option { Value = x.Id, Label = x.Name }).ToListAsync(cancellationToken),
            Brands = await _context.Brands.Select(x => new Option { Value = x.Id, Label = x.Name }).ToListAsync(cancellationToken)
        };
        return Result.Success(data);
    }
}
