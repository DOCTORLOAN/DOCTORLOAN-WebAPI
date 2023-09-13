using AutoMapper;
using AutoMapper.QueryableExtensions;
using DoctorLoan.Application;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Customer.Application.Features.Customers.Dtos;
using DoctorLoan.Domain.Enums.Bookings;
using DoctorLoan.Domain.Enums.Commons;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.Customer.Application.Features.Customers;

public class GetCustomerBookingQuery : QueryParam, IRequest<Result<PaginatedList<CustomerBookingDto>>>
{
    public BookingType Type { get; set; }
    public BookingStatus? Status { get; set; }
}

public class FilterBookingQueryHandler : ApplicationBaseService<FilterCustomerQueryHandler>, IRequestHandler<GetCustomerBookingQuery, Result<PaginatedList<CustomerBookingDto>>>
{
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    public FilterBookingQueryHandler(ILogger<FilterCustomerQueryHandler> logger,
                                 IApplicationDbContext context,
                                 ICurrentRequestInfoService currentRequestInfoService,
                                 ICurrentTranslateService currentTranslateService,
                                 ICurrentUserService currentUserService,
                                 IDateTime dateTime, IMapper mapper)
        : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<Result<PaginatedList<CustomerBookingDto>>> Handle(GetCustomerBookingQuery request, CancellationToken cancellationToken)
    {
        var queryable = _context.Bookings
                                .Include(s => s.Customer)
                                .Include(s => s.CustomerAddresses)
                                .Where(s => s.CustomerId == _currentUserService.UserId
                                            && s.Type == request.Type
                                            && request.Status == null || request.Status.Value == s.Status);

        queryable = request.Params.sort.Trim().ToLower() switch
        {
            "name" => queryable.Sort(m => m.Customer.FullName, request.SortAsc),
            "date" => queryable.Sort(m => m.BookingDate, request.SortAsc),
            _ => queryable.Sort(m => m.Id, asc: false),
        };
        var (page, take, sort, asc) = request.Params;
        var result = await queryable.ProjectTo<CustomerBookingDto>(_mapper.ConfigurationProvider)
                                    .ToPagedListAsync(page, take, cancellationToken);
        return Result.Success(result);
    }
}
