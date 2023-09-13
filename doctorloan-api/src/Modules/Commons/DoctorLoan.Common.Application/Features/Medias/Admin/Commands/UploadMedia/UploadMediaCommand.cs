using DoctorLoan.Application;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Interfaces.Medias;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Application.Models.Settings;
using DoctorLoan.Domain.Enums.Medias;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DoctorLoan.Common.Application.Features.Medias.Admin.Commands;
public class UploadMediaCommand:IRequest<Result<string>>
{
    public required IFormFile File { get; set; }
    public MediaType Type { get; set; }
}
public class UploadMediaCommandHandle : ApplicationBaseService<UploadMediaCommandHandle>, IRequestHandler<UploadMediaCommand, Result<string>>
{
    private readonly IMediaService _mediaService;
    private readonly StorageConfiguration _storageConfiguration;
    public UploadMediaCommandHandle(ILogger<UploadMediaCommandHandle> logger, IApplicationDbContext context, ICurrentRequestInfoService currentRequestInfoService, ICurrentTranslateService currentTranslateService, IDateTime dateTime, IMediaService mediaService, IOptions<StorageConfiguration> storageConfigoptions) : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
        _mediaService = mediaService;
        _storageConfiguration = storageConfigoptions.Value;
    }

    public async Task<Result<string>> Handle(UploadMediaCommand request, CancellationToken cancellationToken)
    {
        using var ms = new MemoryStream();
        await  request.File.CopyToAsync(ms, cancellationToken);
        var media = await _mediaService.UploadMediaAsync(ms.ToArray(), request.File.FileName, request.Type, new List<int> { _storageConfiguration.ImageSizes.Large });
        return Result.Success(media.GetMediaUrl(size: _storageConfiguration.ImageSizes.Large));
    }
}
