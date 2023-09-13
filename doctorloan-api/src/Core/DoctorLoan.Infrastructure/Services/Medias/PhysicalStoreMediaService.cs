using System.IO;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Interfaces.Medias;
using DoctorLoan.Domain.Entities.Medias;
using DoctorLoan.Domain.Enums.Medias;
using Microsoft.Extensions.Logging;
using SkiaSharp;

namespace DoctorLoan.Infrastructure.Services.Medias;
public class PhysicalStoreMediaService : MediaService, IMediaService
{
    private readonly IWebHelper _webHelper;
    public PhysicalStoreMediaService(
        IWebHelper webHelper,
        ILogger<MediaService> logger, 
        IApplicationDbContext context, ICurrentRequestInfoService currentRequestInfoService, ICurrentTranslateService currentTranslateService, IDateTime dateTime) : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
        _webHelper= webHelper;
    }

    public async Task<bool> DeleteMediaAsync(Media picture)
    {
        try
        {
            var fileInfo = new FileInfo(_webHelper.MapPath(picture.Path));
            if (fileInfo.Exists)
                fileInfo.Delete();
            await base.DeleteMediaAsync(picture, CancellationToken.None);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<Media> UploadMediaAsync(byte[] fileBinary,string fileName, MediaType fileType, List<int> listTargetSize,string subFolder=null)
    {
        var fileInfo = GetFileInfo(fileName, fileType, 0, subFolder);
        var fullFolderPath= _webHelper.MapPath(fileInfo.folderPath);
        string fullFilePath =_webHelper.MapPath($"{fullFolderPath}/{fileInfo.fileName}");
        var folderInfo = new DirectoryInfo(fullFolderPath);
        if (!folderInfo.Exists)
            folderInfo.Create();
        foreach (var targetSize in listTargetSize)
        {

            string fileBySize = $"{(targetSize > 0 ? targetSize + "_" : "")}{fileInfo.fileName}";
            var fullFilePathBySize = $"{fullFolderPath}/{fileBySize}";
            using var mutex = new Mutex(false,Path.GetFileNameWithoutExtension(fileBySize));
            mutex.WaitOne();
            try
            {
                using var image = SKBitmap.Decode(fileBinary);
               
                byte[] pictureBinary;
                var format = GetImageFormatByMimeType(fileBySize);
                if (targetSize > 0)
                {
                    pictureBinary = ImageResize(image, format, targetSize);
                }
                else
                {
                    pictureBinary = fileBinary;
                }
               
                File.WriteAllBytes(fullFilePathBySize, pictureBinary);             
               
              
            }
            catch
            {
                mutex.ReleaseMutex();
                break;
            }


        }       
        var media = GetMediaInfo(fileInfo.fileName, fileInfo.folderPath, fileName, fileType);
        await InsertMediaAsync(media);
        return media;


    }

}
