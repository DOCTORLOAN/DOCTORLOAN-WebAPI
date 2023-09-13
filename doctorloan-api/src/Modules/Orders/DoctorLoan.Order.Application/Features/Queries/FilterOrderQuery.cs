using AutoMapper;
using AutoMapper.QueryableExtensions;
using DoctorLoan.Application;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Domain.Enums.Commons;
using DoctorLoan.Domain.Enums.Orders;
using DoctorLoan.Order.Application.Commons.Expressions;
using DoctorLoan.Order.Application.Features.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.Order.Application.Features.Queries;

public class FilterOrderQuery : QueryParam, IRequest<Result<PaginatedList<OrderDto>>>
{
    [FromQuery(Name = "search")]
    public string? Search { get; set; }

    public RequestFilter Filter { get; set; } = new RequestFilter();
}

public class RequestFilter : IFilter
{
    public OrderStatus? Status { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
}

public class FilterOrderQueryHandler : ApplicationBaseService<FilterOrderQueryHandler>, IRequestHandler<FilterOrderQuery, Result<PaginatedList<OrderDto>>>
{
    private readonly IMapper _mapper;
    public FilterOrderQueryHandler(ILogger<FilterOrderQueryHandler> logger,
                                 IApplicationDbContext context,
                                 ICurrentRequestInfoService currentRequestInfoService,
                                 ICurrentTranslateService currentTranslateService,
                                 IDateTime dateTime, IMapper mapper)
        : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
        _mapper = mapper;
    }

    public async Task<Result<PaginatedList<OrderDto>>> Handle(FilterOrderQuery request, CancellationToken cancellationToken)
    {
        var condition = PredicateBuilder.True<Domain.Entities.Orders.Order>();

        #region add conditions
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            condition = condition.And(OrderExpression.IsContains(request.Search));
        }

        if (request.Filter.Status.HasValue)
        {
            condition = condition.And(s => s.Status == request.Filter.Status);
        }

        if (request.Filter.PaymentMethod.HasValue)
        {
            condition = condition.And(s => s.PaymentMethod == request.Filter.PaymentMethod);
        }
        #endregion

        var queryable = _context.Orders.Where(condition);
        queryable = request.Params.sort.Trim().ToLower() switch
        {
            "orderNo" => queryable.Sort(m => m.OrderNo, request.SortAsc),
            "name" => queryable.Sort(m => m.FullName, request.SortAsc),
            "date" => queryable.Sort(m => m.Created, request.SortAsc),
            _ => queryable.Sort(m => m.Id, asc: false),
        };
        var (page, take, sort, asc) = request.Params;
        var result = await queryable.ProjectTo<OrderDto>(_mapper.ConfigurationProvider)
                                    .ToPagedListAsync(page, take, cancellationToken);
        return Result.Success(result);
    }
}