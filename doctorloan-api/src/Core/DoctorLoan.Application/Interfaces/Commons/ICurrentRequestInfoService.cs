using DoctorLoan.Application.Features.Users.Dtos;
using DoctorLoan.Domain.Enums.Commons;

namespace DoctorLoan.Application.Interfaces.Commons;

public interface ICurrentRequestInfoService
{
    LanguageEnum Language { get; }
    public SourcePlatform SourcePlatform { get; }
    public string UserAgent { get; }
    public string FullBrowserInfo();
    public UserDeviceInfo DeviceInfo();
    string Signature { get; }
}
