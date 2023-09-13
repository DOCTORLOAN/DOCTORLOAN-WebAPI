using DoctorLoan.Application.Models.Medias;
using DoctorLoan.Domain.Enums.Medias;
using Microsoft.AspNetCore.Http;

namespace DoctorLoan.Application.Interfaces.StorageServices;
public interface IStorageService
{
    Task<BlobDto> GetAsync(string fileName, string extention, string contentType, MediaType type, ImageSize? size = null);

    Task<List<BlobDto>> GetAllAsync(MediaType type);

    Task<BlobDto> UploadAsync(IFormFile blob, MediaType type);

    Task<bool> DeleteAsync(string fileName, MediaType type);

    Task<BlobDto> GetNoImageDefault();

    Task<MediaInfo> UploadFileAsync(IFormFile blob, MediaType type, CancellationToken cancellationToken = default);

    string GetBlobFile(string containerName, string fileName);
}
