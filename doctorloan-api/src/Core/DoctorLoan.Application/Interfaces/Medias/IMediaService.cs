using DoctorLoan.Domain.Entities.Medias;
using DoctorLoan.Domain.Enums.Medias;

namespace DoctorLoan.Application.Interfaces.Medias;
public partial interface IMediaService
{
    Task<string> GetMediaUrlAsync(int mediaId,
       int targetSize = 0,
       bool showDefaultPicture = true,
       string storeLocation = null);
    Task<Media> UploadMediaAsync(byte[] fileBinary, string fileName, MediaType fileType, List<int> listTargetSize, string subFolder = null);
    string GetDefaultImageUrl(string storeLocation = null);
    Task<bool> DeleteMediaAsync(Media picture);

}
