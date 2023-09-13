using DoctorLoan.Application;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Domain.Enums.Commons;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.Booking.Application.Features.Commands;
public record UpdateBookingCommand(int Id) : IRequest<Result<bool>>
{
    public BookingStatus Status { get; set; }
    public string Noted { get; set; }
}

public class UpdateBookingCommandHandler : ApplicationBaseService<AddBookingCommandHandler>, IRequestHandler<UpdateBookingCommand, Result<bool>>
{
    public UpdateBookingCommandHandler(ILogger<AddBookingCommandHandler> logger, IApplicationDbContext context,
                                    ICurrentRequestInfoService currentRequestInfoService,
                                    ICurrentTranslateService currentTranslateService,
                                    IDateTime dateTime)
    : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    { }

    public async Task<Result<bool>> Handle(UpdateBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await _context.Bookings.FindAsync(new object[] { request.Id }, cancellationToken);
        if (booking is null) return Result.Failed<bool>(ServiceError.NotFound(_currentTranslateService));
        booking.Status = request.Status;
        booking.Noted = request.Noted;
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(true);
    }
}