using AutoMapper;
using AutoMapper.QueryableExtensions;
using DoctorLoan.Application;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Customer.Application.Commons.Expressions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.Customer.Application.Features.Customers;

public class FilterCustomerQuery : QueryParam, IRequest<Result<PaginatedList<CustomerDto>>>
{
    public string Search { get; set; }
}

public class FilterCustomerQueryHandler : ApplicationBaseService<FilterCustomerQueryHandler>, IRequestHandler<FilterCustomerQuery, Result<PaginatedList<CustomerDto>>>
{
    private readonly IMapper _mapper;
    public FilterCustomerQueryHandler(ILogger<FilterCustomerQueryHandler> logger,
                                 IApplicationDbContext context,
                                 ICurrentRequestInfoService currentRequestInfoService,
                                 ICurrentTranslateService currentTranslateService,
                                 IDateTime dateTime, IMapper mapper)
        : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
        _mapper = mapper;
    }

    public async Task<Result<PaginatedList<CustomerDto>>> Handle(FilterCustomerQuery request, CancellationToken cancellationToken)
    {
        var condition = PredicateBuilder.Create<Domain.Entities.Customers.Customer>(s => s.IsDelete == false);

        #region add conditions
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            condition = condition.And(CustomerExpression.IsContains(request.Search));
        }
        #endregion

        var queryable = _context.Customers.Where(condition);
        queryable = request.Params.sort.Trim().ToLower() switch
        {
            "name" => queryable.Sort(m => m.FullName, request.SortAsc),
            _ => queryable.Sort(m => m.Id, asc: false),
        };
        var (page, take, sort, asc) = request.Params;
        var result = await queryable.ProjectTo<CustomerDto>(_mapper.ConfigurationProvider)
                                    .ToPagedListAsync(page, take, cancellationToken);
        return Result.Success(result);
    }
}