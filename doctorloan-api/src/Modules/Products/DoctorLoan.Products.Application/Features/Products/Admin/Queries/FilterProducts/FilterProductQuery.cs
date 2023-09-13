using AutoMapper;
using DoctorLoan.Application;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Domain.Entities.Products;
using DoctorLoan.Domain.Enums.Commons;
using DoctorLoan.Products.Application.Features.Products.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.Products.Application.Features.Products.Admin.Queries;
public class FilterProductQuery : QueryParam, IRequest<Result<PaginatedList<ProductFilterResultDto>>>
{
    public string? Keyword { get; set; }
    public StatusEnum? Status { get; set; }
    public int? CategoryId { get; set; }
}
public class FilterProductQueryHandle : ApplicationBaseService<FilterProductQueryHandle>, IRequestHandler<FilterProductQuery, Result<PaginatedList<ProductFilterResultDto>>>
{
    private readonly IMapper _mapper;
    public FilterProductQueryHandle(IMapper mapper, ILogger<FilterProductQueryHandle> logger, IApplicationDbContext context, ICurrentRequestInfoService currentRequestInfoService, ICurrentTranslateService currentTranslateService, IDateTime dateTime) : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
        _mapper = mapper;
    }

    public async Task<Result<PaginatedList<ProductFilterResultDto>>> Handle(FilterProductQuery request, CancellationToken cancellationToken)
    {
        var condition = PredicateBuilder.True<Product>();
        if (request.Keyword != null)
            condition = condition.And(x => x.Sku.ToLower().Contains(request.Keyword.ToLower()) || x.Name.ToLower().Contains(request.Keyword.ToLower()));
        if (request.Status.HasValue)
        {
            condition = condition.And(x => x.Status == request.Status);
        }
        if (request.CategoryId.HasValue)
            condition = condition.And(x => x.ProductCategories.Any(c => c.CategoryId == request.CategoryId));
        var query = _context.Products.Where(condition)
                                    .OrderByDescending(x => x.Status)
                                        .ThenByDescending(s => s.LastModified);
        var data = await _mapper.ProjectTo<ProductFilterResultDto>(query)
            .ToPagedListAsync(request.Page, request.Take, cancellationToken);
        return Result.Success(data);
    }
}