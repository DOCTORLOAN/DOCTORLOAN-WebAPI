using System.Text;
using Newtonsoft.Json;

namespace DoctorLoan.Application.Common.Extentions;
public static class HttpClientExtensions
{
    public static StringContent ToJsonContent<T>(this T obj)
    {
        return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
    }
}
