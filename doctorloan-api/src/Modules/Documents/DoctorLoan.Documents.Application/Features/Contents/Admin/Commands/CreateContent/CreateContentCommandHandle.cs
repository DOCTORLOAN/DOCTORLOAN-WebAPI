using DoctorLoan.Application;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Domain.Entities.Contents;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.Contents.Application.Features.Contents.Admin.Commands;
public class CreateContentCommandHandle : ApplicationBaseService<CreateContentCommandHandle>, IRequestHandler<CreateContentCommand, Result<bool>>
{

    private readonly IMapper _mapper;

    public CreateContentCommandHandle(ILogger<CreateContentCommandHandle> logger,
                             IApplicationDbContext context,
                             IMapper mapper,
                             ICurrentRequestInfoService currentRequestInfoService,
                             ICurrentTranslateService currentTranslateService,
                             IDateTime dateTime)
    : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
        _mapper = mapper;
    }

    public async Task<Result<bool>> Handle(CreateContentCommand command, CancellationToken cancellationToken)
    {
        var isExsitedDocumentCode = await _context.Contents.AnyAsync(a => a.Code == command.Code, cancellationToken);
        if (isExsitedDocumentCode)
            return Result.Failed<bool>(ServiceError.CodeExisted(_currentTranslateService));

        var info = _mapper.Map<Content>(command);
        await _context.Contents.AddAsync(info, cancellationToken);

        var result = await _context.SaveChangesAsync(cancellationToken) > 0;
        return Result.Success(result);
    }
}
