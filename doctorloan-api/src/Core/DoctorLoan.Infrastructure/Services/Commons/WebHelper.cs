using DoctorLoan.Application.Interfaces.Commons;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace DoctorLoan.Infrastructure.Services.Commons;
public class WebHelper : IWebHelper
{

    #region Fields
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
    #endregion
    #region Ctor
    public WebHelper(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
    {
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
    }
    #endregion
    #region Methods

    #endregion
    #region Privates
    #endregion
    public bool IsCurrentConnectionSecured()
    {
        return _httpContextAccessor.HttpContext.Request.IsHttps;
    }
    public string GetApiHost()
    {
        bool useSsl = IsCurrentConnectionSecured();
        //try to get host from the request HOST header
        var hostHeader = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Host];
        if (StringValues.IsNullOrEmpty(hostHeader))
            return string.Empty;

        //add scheme to the URL
        var storeHost = $"{(useSsl ? Uri.UriSchemeHttps : Uri.UriSchemeHttp)}{Uri.SchemeDelimiter}{hostHeader.FirstOrDefault()}";

        //ensure that host is ended with slash
        storeHost = $"{storeHost.TrimEnd('/')}/";

        return storeHost;
    }
   
    public  string MapPath(string path)
    {
        path = path.Replace("~/", string.Empty).TrimStart('/');
        return _webHostEnvironment.WebRootPath+"/" + path;
    }
}
