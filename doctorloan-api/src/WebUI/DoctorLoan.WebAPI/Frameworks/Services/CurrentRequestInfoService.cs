using DoctorLoan.Application.Features.Users.Dtos;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Domain.Enums.Commons;
using UAParser;

namespace DoctorLoan.WebAPI.Frameworks.Services;

public class CurrentRequestInfoService : ICurrentRequestInfoService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentRequestInfoService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string Signature => _httpContextAccessor.HttpContext?.Request.Headers["signature"].ToString();

    public LanguageEnum Language => _httpContextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString() == "en-US" ? LanguageEnum.EN : LanguageEnum.VN;

    public SourcePlatform SourcePlatform => Enum.TryParse(_httpContextAccessor.HttpContext?.Request.Headers["SourcePlatform"].ToString(), out SourcePlatform current) ? current : SourcePlatform.Unknown;
    public string UserAgent => _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString();

    public string FullBrowserInfo()
    {
        var uaParser = Parser.GetDefault();
        ClientInfo c = uaParser.Parse(_httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString());
        dynamic myDynamic = new System.Dynamic.ExpandoObject();

        if (c != null)
        {
            myDynamic.Family = c.UA?.Family;
            myDynamic.Major = c.UA?.Major;
            myDynamic.Minor = c.UA?.Minor;
            myDynamic.Patch = c.UA?.Patch;

            myDynamic.OSFamily = c.OS?.Family;
            myDynamic.OSMajor = c.OS?.Major;
            myDynamic.OSMinor = c.OS?.Minor;
            myDynamic.OSPatch = c.OS?.Patch;

            myDynamic.DeviceFamily = c.Device?.Family;
            myDynamic.DeviceModel = c.Device?.Model;
            myDynamic.DeviceBrand = c.Device?.Brand;
            myDynamic.DeviceIsSpider = c.Device?.IsSpider;
        }

        var json = Newtonsoft.Json.JsonConvert.SerializeObject(myDynamic);

        return json;
    }

    public UserDeviceInfo DeviceInfo()
    {
        var request = _httpContextAccessor.HttpContext?.Request;
        if (request == null)
        {
            return null;
        }
        var platform = request.Headers["Platform"].ToString();
        var DeviceId = request.Headers["Device-Id"].ToString();
        var DeviceName = request.Headers["Device-Name"].ToString();
        var AppVersion = request.Headers["Version-App"].ToString();
        var DeviceInfo = request.Headers["User-Agent"].ToString();
        var result = new UserDeviceInfo()
        {
            Platform = Enum.TryParse(platform, out PlatformType current) ? current : PlatformType.WEBSITE,
            DeviceId = DeviceId,
            DeviceName = DeviceName,
            VersionApp = AppVersion,
            Version = DeviceInfo,
        };
        return result;
    }

}