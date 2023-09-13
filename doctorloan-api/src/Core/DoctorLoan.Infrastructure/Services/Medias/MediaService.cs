using DoctorLoan.Application;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Domain.Entities.Medias;
using DoctorLoan.Domain.Enums.Medias;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkiaSharp;

namespace DoctorLoan.Infrastructure.Services.Medias;
public class MediaService : ApplicationBaseService<MediaService>
{
    public MediaService(ILogger<MediaService> logger, IApplicationDbContext context, ICurrentRequestInfoService currentRequestInfoService, ICurrentTranslateService currentTranslateService, IDateTime dateTime) : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
    }
    
   

    public async Task<string> GetMediaUrlAsync(int mediaId, int targetSize = 0, bool showDefaultPicture = true, string storeLocation = null)
    {
        var media = await _context.Medias.FirstOrDefaultAsync(x => x.Id == mediaId);
        if(media != null)
        {
            return $"{storeLocation}/{media.Path}{(targetSize>0?targetSize:"")}{media.Name}";
        }
        if (showDefaultPicture)
            return GetDefaultImageUrl(storeLocation);
        return string.Empty;
            
    }
    public string GetDefaultImageUrl(string storeLocation = null)
    {
        return $"{storeLocation}/images/placeholder-image.jpg";
    }
    public (string folderPath,string fileName) GetFileInfo(string fileName,MediaType mediaType,int size=0,string subFolder=null)
    {
        string _fileName=Path.GetFileNameWithoutExtension(fileName);
        string _fileExt=Path.GetExtension(fileName);
        string _random=Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
        string fullFileName = $"{(size > 0 ?  size+"_" : "")}{_fileName}_{_random}{_fileExt}";

        return ($"/DataFiles/{mediaType.ToString().ToLower()}{(subFolder!=null?"/"+subFolder:"")}",fullFileName);
    }
    protected Media GetMediaInfo(string fileName,string folderPath,string originFileName,MediaType mediaType,bool hasStorage=false)
    {
        return new Media
        {
            ContentType = "image/jpeg",
            Extention = Path.GetExtension(fileName),
            HasStorage= hasStorage,
            OriginalName= originFileName,
            Name= fileName,
            Type=mediaType,
            Path= folderPath,
            Status=MediaStatus.Active,
            
        };
    }
    protected async Task DeleteMediaAsync(Media picture, CancellationToken cancellationToken)
    {
        _context.Medias.Remove(picture);
        await _context.SaveChangesAsync(cancellationToken);
    }
    protected async Task InsertMediaAsync(Media media)
    {
        _context.Medias.Add(media);
        await _context.SaveChangesAsync(CancellationToken.None);
    }
    protected virtual byte[] ImageResize(SKBitmap image, SKEncodedImageFormat format, int targetSize)
    {
        if (image == null)
            throw new ArgumentNullException("Image is null");

        float width, height;
        if (image.Height > image.Width)
        {
            // portrait
            width = image.Width * (targetSize / (float)image.Height);
            height = targetSize;
        }
        else
        {
            // landscape or square
            width = targetSize;
            height = image.Height * (targetSize / (float)image.Width);
        }
        
        if ((int)width == 0 || (int)height == 0)
        {
            width = image.Width;
            height = image.Height;
        }
        try
        {
            using var resizedBitmap = image.Resize(new SKImageInfo((int)width, (int)height), SKFilterQuality.Medium);
            using var cropImage = SKImage.FromBitmap(resizedBitmap);

            //In order to exclude saving pictures in low quality at the time of installation, we will set the value of this parameter to 80 (as by default)
            return cropImage.Encode(format, 80).ToArray();
        }
        catch
        {
            return image.Bytes;
        }

    }
    protected virtual SKEncodedImageFormat GetImageFormatByMimeType(string mimeType)
    {
        var format = SKEncodedImageFormat.Jpeg;
        if (string.IsNullOrEmpty(mimeType))
            return format;

        var parts = mimeType.ToLower().Split('/');
        var lastPart = parts[^1];

        switch (lastPart)
        {
            case "webp":
                format = SKEncodedImageFormat.Webp;
                break;
            case "png":
            case "gif":
            case "bmp":
            case "x-icon":
                format = SKEncodedImageFormat.Png;
                break;
            default:
                break;
        }

        return format;
    }
}
