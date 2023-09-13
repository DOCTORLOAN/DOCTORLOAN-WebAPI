using Newtonsoft.Json;

namespace DoctorLoan.Domain.Extentions;
public static class JsonExtentions
{
    public static T ToObject<T>(this string jsonString) where T : class
    {
        return JsonConvert.DeserializeObject<T>(jsonString);
    }
    public static string ToJsonString<T>(this T obj) where T : class
    {
        return JsonConvert.SerializeObject(obj);
    }
}
