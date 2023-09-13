using AutoMapper;
using AutoMapper.QueryableExtensions;
using DoctorLoan.Application;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Booking.Application.Commons.Expressions;
using DoctorLoan.Booking.Application.Features.Dtos;
using DoctorLoan.Customer.Application.Features.Customers;
using DoctorLoan.Domain.Enums.Bookings;
using DoctorLoan.Domain.Enums.Commons;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.Booking.Application.Features.Queries;

public class FilterBookingQuery : QueryParam, IRequest<Result<PaginatedList<BookingDto>>>
{
    [FromQuery(Name = "search")]
    public string Search { get; set; }

    public RequestFilter Filter { get; set; } = new RequestFilter();
}

public class RequestFilter : IFilter
{
    public BookingStatus? Status { get; set; }
    public DateOnly? BookingDate { get; set; }
    public BookingType? Type { get; set; }
}

public class FilterBookingQueryHandler : ApplicationBaseService<FilterCustomerQueryHandler>, IRequestHandler<FilterBookingQuery, Result<PaginatedList<BookingDto>>>
{
    private readonly IMapper _mapper;
    public FilterBookingQueryHandler(ILogger<FilterCustomerQueryHandler> logger,
                                 IApplicationDbContext context,
                                 ICurrentRequestInfoService currentRequestInfoService,
                                 ICurrentTranslateService currentTranslateService,
                                 IDateTime dateTime, IMapper mapper)
        : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
        _mapper = mapper;
    }

    public async Task<Result<PaginatedList<BookingDto>>> Handle(FilterBookingQuery request, CancellationToken cancellationToken)
    {
        var condition = PredicateBuilder.True<Domain.Entities.Bookings.Booking>();

        #region add conditions
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            condition = condition.And(BookingExpression.IsContains(request.Search));
        }

        if (request.Filter.Status.HasValue)
        {
            condition = condition.And(s => s.Status == request.Filter.Status);
        }

        if (request.Filter.Type.HasValue)
        {
            condition = condition.And(s => s.Type == request.Filter.Type);
        }

        if (request.Filter.BookingDate.HasValue)
        {
            condition = condition.And(s => s.BookingDate == request.Filter.BookingDate);
        }
        #endregion

        var queryable = _context.Bookings
                                .Include(s => s.Customer)
                                .Include(s => s.CustomerAddresses)
                                .Where(condition);
        queryable = request.Params.sort.Trim().ToLower() switch
        {
            "name" => queryable.Sort(m => m.Customer.FullName, request.SortAsc),
            "date" => queryable.Sort(m => m.BookingDate, request.SortAsc),
            _ => queryable.Sort(m => m.Id, asc: false),
        };
        var (page, take, sort, asc) = request.Params;
        var result = await queryable.ProjectTo<BookingDto>(_mapper.ConfigurationProvider)
                                    .ToPagedListAsync(page, take, cancellationToken);
        return Result.Success(result);
    }
}