using DoctorLoan.Application;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Domain.Enums.Commons;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.Contents.Application.Features.Contents.Admin.Commands.UpdateContent;
public class UpdateContentCommandHandle : ApplicationBaseService<UpdateContentCommandHandle>, IRequestHandler<UpdateContentCommand, Result<bool>>
{
    public UpdateContentCommandHandle(ILogger<UpdateContentCommandHandle> logger,
                                        IApplicationDbContext context,
                                        ICurrentRequestInfoService currentRequestInfoService,
                                        ICurrentTranslateService currentTranslateService,
                                        IDateTime dateTime)
    : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
    }

    public async Task<Result<bool>> Handle(UpdateContentCommand command, CancellationToken cancellationToken)
    {
        var document = await _context.Contents.FirstOrDefaultAsync(x => x.Id == command.Id && x.Status != StatusEnum.Removed, cancellationToken);
        if (document == null)
            return Result.Failed<bool>(ServiceError.NotFound(_currentTranslateService));

        #region Set Value
        if (!string.IsNullOrEmpty(command.Description))
            document.Description = command.Description;

        if (command.MediaId.HasValue)
            document.MediaId = command.MediaId.Value;

        if (document.Status != command.Status)
            document.Status = command.Status;

        document.Name = command.Name;
        document.Type = command.Type;
        document.Code = command.Code;
        #endregion

        _context.Contents.Update(document);
        var result = await _context.SaveChangesAsync(cancellationToken) > 0;

        return Result.Success(result);
    }
}
