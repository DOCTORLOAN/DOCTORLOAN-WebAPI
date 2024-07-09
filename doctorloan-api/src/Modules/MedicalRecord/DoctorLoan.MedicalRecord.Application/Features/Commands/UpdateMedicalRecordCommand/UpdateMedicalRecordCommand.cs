using DoctorLoan.Application;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Domain.Enums.Commons;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.MedicalRecord.Application.Features.Commands;
public record UpdateMedicalRecordCommand(int Id) : IRequest<Result<bool>>
{
    public StatusEnum Status { get; set; }
    public string Noted { get; set; }
}

public class UpdateMedicalRecordCommandHandler : ApplicationBaseService<AddMedicalRecordCommandHandler>, IRequestHandler<UpdateMedicalRecordCommand, Result<bool>>
{
    public UpdateMedicalRecordCommandHandler(ILogger<AddMedicalRecordCommandHandler> logger, IApplicationDbContext context,
                                    ICurrentRequestInfoService currentRequestInfoService,
                                    ICurrentTranslateService currentTranslateService,
                                    IDateTime dateTime)
    : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    { }

    public async Task<Result<bool>> Handle(UpdateMedicalRecordCommand request, CancellationToken cancellationToken)
    {
        var medicalRecord = await _context.MedicalRecords.FindAsync(new object[] { request.Id }, cancellationToken);
        if (medicalRecord is null) return Result.Failed<bool>(ServiceError.NotFound(_currentTranslateService));
         medicalRecord.Status = request.Status;
        medicalRecord.Noted = request.Noted;
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(true);
    }
}
