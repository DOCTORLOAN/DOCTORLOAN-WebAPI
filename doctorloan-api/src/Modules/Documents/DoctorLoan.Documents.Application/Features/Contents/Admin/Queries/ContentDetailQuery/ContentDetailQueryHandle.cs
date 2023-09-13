using DoctorLoan.Application;
using DoctorLoan.Application.Common.Exceptions;
using DoctorLoan.Application.Common.Helpers;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Contents.Application.Features.Contents.Admin.Dtos;
using DoctorLoan.Domain.Enums.Contents;
using DoctorLoan.Domain.Enums.Medias;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.Contents.Application.Features.Contents.Admin;

public record ContentDetailQuery(int Id) : IRequest<Result<ContentInfomationDto>>;
public class ContentDetailQueryHandle : ApplicationBaseService<ContentDetailQueryHandle>, IRequestHandler<ContentDetailQuery, Result<ContentInfomationDto>>
{
    public ContentDetailQueryHandle(ILogger<ContentDetailQueryHandle> logger,
                             IApplicationDbContext context,
                             ICurrentRequestInfoService currentRequestInfoService,
                             ICurrentTranslateService currentTranslateService,
                             IDateTime dateTime)
    : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
    }

    public async Task<Result<ContentInfomationDto>> Handle(ContentDetailQuery request, CancellationToken cancellationToken)
    {
        var content = await _context.Contents.FindAsync(new object[] { request.Id }, cancellationToken);
        if (content == null)
        {
            throw new NotFoundException("Document", request.Id);
        }

        var result = new ContentInfomationDto(content);
        if (content.MediaId.HasValue && content.MediaId.Value > 0)
        {
            var media = await _context.Medias.FirstOrDefaultAsync(c => c.Id == content.MediaId.Value, cancellationToken);
            if (media == null)
            {
                throw new NotFoundException("Media", content.MediaId.Value);
            }

            result.Media = new MediaInfoDto
            {
                Id = content.MediaId.Value,
                Name = media.OriginalName,
                Path = content.Type == ContentTypeEnum.Video ? media.Path : CommonHelper.GetMediaUrlById(media.Id, ImageSize.ExtraSmall),
                Type = media.Type
            };
        }
        return Result.Success(result);
    }
}
