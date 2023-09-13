using AutoMapper;
using DoctorLoan.Application;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Booking.Application.Features.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.Booking.Application.Features.Queries;

public record GetBookingByIdQuery(int Id) : IRequest<Result<BookingDto>> { }

public class GetBookingByIdQueryHandler : ApplicationBaseService<GetBookingByIdQueryHandler>, IRequestHandler<GetBookingByIdQuery, Result<BookingDto>>
{
    private readonly IMapper _mapper;
    public GetBookingByIdQueryHandler(ILogger<GetBookingByIdQueryHandler> logger,
                                 IApplicationDbContext context,
                                 ICurrentRequestInfoService currentRequestInfoService,
                                 ICurrentTranslateService currentTranslateService,
                                 IDateTime dateTime, IMapper mapper)
        : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
        _mapper = mapper;
    }

    public async Task<Result<BookingDto>> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
    {
        var info = await _context.Bookings.Include(s => s.Customer)
                                            .Include(s => s.CustomerAddresses)
                                                .ThenInclude(s => s.Address)
                                        .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (info == null) return Result.Failed<BookingDto>(ServiceError.NotFound(_currentTranslateService));
        var data = _mapper.Map<BookingDto>(info);

        return Result.Success(data);
    }
}