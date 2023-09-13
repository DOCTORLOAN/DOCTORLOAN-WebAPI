using AutoMapper;
using AutoMapper.QueryableExtensions;
using DoctorLoan.Application;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Customer.Application.Commons.Expressions;
using DoctorLoan.Customer.Application.Features.CustomerAddresses.Dtos;
using DoctorLoan.Domain.Entities.Customers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.Customer.Application.Features.CustomerAddresses;


public class FilterCustomerAddressQuery : QueryParam, IRequest<Result<PaginatedList<CustomerAddressDto>>>
{
    public int CustomerId { get; set; }

    [FromQuery(Name = "filter_search")]
    public string Search { get; set; }
}

public class FilterCustomerQueryHandler : ApplicationBaseService<FilterCustomerQueryHandler>, IRequestHandler<FilterCustomerAddressQuery, Result<PaginatedList<CustomerAddressDto>>>
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

    public async Task<Result<PaginatedList<CustomerAddressDto>>> Handle(FilterCustomerAddressQuery request, CancellationToken cancellationToken)
    {
        var condition = PredicateBuilder.Create<CustomerAddress>(s => s.CustomerId == request.CustomerId);

        #region add conditions
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            condition = condition.And(CustomerExpression.AddressContain(request.Search));
        }
        #endregion

        var queryable = _context.CustomerAddresses.Include(s => s.Address).Where(condition);
        queryable = request.Params.sort.Trim().ToLower() switch
        {
            "name" => queryable.Sort(m => m.FullName, request.SortAsc),
            _ => queryable.Sort(m => m.Id, asc: false),
        };
        var (page, take, sort, asc) = request.Params;
        var result = await queryable.ProjectTo<CustomerAddressDto>(_mapper.ConfigurationProvider)
                                  .ToPagedListAsync(page, take, cancellationToken);
        return Result.Success(result);
    }
}
