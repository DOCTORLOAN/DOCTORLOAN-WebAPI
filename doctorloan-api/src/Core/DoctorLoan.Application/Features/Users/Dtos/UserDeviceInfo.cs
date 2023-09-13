
using DoctorLoan.Domain.Enums.Commons;

namespace DoctorLoan.Application.Features.Users.Dtos;
public class UserDeviceInfo
{
    public PlatformType Platform { get; set; }
    public string DeviceId { get; set; }
    public string DeviceName { get; set; }
    public string Version { get; set; }
    public string VersionApp { get; set; }
}
