using System.ComponentModel;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Medias;
using DoctorLoan.Application.Models.Settings;
using DoctorLoan.Domain.Entities.Medias;
using DoctorLoan.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DoctorLoan.Application.Common.Extentions;

public static class DataExtensions
{
    public static string MonthYearCode(int month, int year)
    {
        string monthText = "A";
        string yearText = "A";

        if (month == 1) monthText = "A";
        else if (month == 2) monthText = "B";
        else if (month == 3) monthText = "C";
        else if (month == 4) monthText = "D";
        else if (month == 5) monthText = "E";
        else if (month == 6) monthText = "F";
        else if (month == 7) monthText = "G";
        else if (month == 8) monthText = "H";
        else if (month == 9) monthText = "I";
        else if (month == 10) monthText = "J";
        else if (month == 11) monthText = "K";
        else if (month == 12) monthText = "L";

        var alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        int count = 0;
        var listYearText = new List<Tuple<int, string>>();
        foreach (char c in alpha)
        {
            listYearText.Add(new Tuple<int, string>(2023 + count, c.ToString()));
            count++;
        }
        yearText = listYearText.FirstOrDefault(x => x.Item1 == year)?.Item2;

        return yearText + monthText;
    }
    public static string GetUniqueAlphanumericKey(int maxSize)
    {
        _ = new char[62];
        char[] chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
        byte[] data = new byte[1];
#pragma warning disable SYSLIB0023 // Type or member is obsolete
        RNGCryptoServiceProvider crypto = new();
#pragma warning restore SYSLIB0023 // Type or member is obsolete
        crypto.GetNonZeroBytes(data);
        data = new byte[maxSize];
        crypto.GetNonZeroBytes(data);
        StringBuilder result = new(maxSize);
        foreach (byte b in data)
        {
            result.Append(chars[b % (chars.Length)]);
        }
        return result.ToString();
    }
    public static string GetUniqueKey(int maxSize)
    {
        _ = new char[62];
        char[] chars = "0123456789".ToCharArray();
        byte[] data = new byte[1];
#pragma warning disable SYSLIB0023 // Type or member is obsolete
        RNGCryptoServiceProvider crypto = new();
#pragma warning restore SYSLIB0023 // Type or member is obsolete
        crypto.GetNonZeroBytes(data);
        data = new byte[maxSize];
        crypto.GetNonZeroBytes(data);
        StringBuilder result = new(maxSize);
        foreach (byte b in data)
        {
            result.Append(chars[b % (chars.Length)]);
        }
        return result.ToString();
    }

    public static string GetExceptionMessages(Exception e, string msgs = "")
    {
        if (e == null) return string.Empty;
        if (msgs == "") msgs = e.Message;
        if (e.InnerException != null)
            msgs += "\r\nInnerException: " + GetExceptionMessages(e.InnerException);
        return msgs;
    }

    public static void AddRange<T>(this ICollection<T> collections, IEnumerable<T> items)
    {
        foreach (var item in items)
            collections.Add(item);
    }
    public static void RemoveRange<T>(this ICollection<T> collections, IEnumerable<T> items)
    {
        foreach (var item in items)
            collections.Remove(item);
    }
    public static void RemoveWhen<T>(this ICollection<T> collections, Func<T, bool> predicate)
    {
        var removeItems = collections.Where(x => predicate(x)).ToList();
        foreach (var item in removeItems)
            collections.Remove(item);
    }

    public static string GetMediaUrl(this Media media, bool includeHostName = true, int size = 0)
    {
        var host = EngineContext.GetService<IOptions<PortalConfiguration>>().Value?.MediaStorageHost;
        host = host ?? EngineContext.GetService<IWebHelper>().GetApiHost().TrimEnd('/');
        if (media == null)
            return EngineContext.GetService<IMediaService>().GetDefaultImageUrl(includeHostName ? host : "");
        string url = $"{media.Path}/{(size > 0 ? size+"_" : "")}{media.Name}";
        if (includeHostName)
            return $"{host}{url}";
        return url;
    }
    public static string GetMediaPortalUrl(this Media media, int size = 0)
    {
        var host = EngineContext.GetService<IOptions<PortalConfiguration>>().Value.MediaStorageHost;
        if (media == null)
            return EngineContext.GetService<IMediaService>().GetDefaultImageUrl(host);
        string url = $"{media.Path}/{(size > 0 ? size + "_" : "")}{media.Name}";
        return $"{host}{url}";
    }

    public static string GetDescription<T>(this T enumValue) where T : struct, IConvertible
    {
        if (!typeof(T).IsEnum)
            return null;
        return GetDescription(enumValue as object);
    }
    public static string GetDescription(this object @obj)
    {

        var description = @obj.ToString();
        var fieldInfo = @obj.GetType().GetField(@obj.ToString());

        if (fieldInfo != null)
        {
            var attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (attrs != null && attrs.Length > 0)
            {
                description = ((DescriptionAttribute)attrs[0]).Description;
            }
        }

        return description;
    }

    public static string GetFullUserAddress(this UserAddress userAddress)
    {
        return (userAddress != null ? string.Join(", ", new List<string> { userAddress.Address.AddressLine, $"{userAddress.Address.Ward.Type} {userAddress.Address.Ward.Name}", $"{userAddress.Address.District.Type} {userAddress.Address.District.Name}", $"{userAddress.Address.Province.Type} {userAddress.Address.Province.Name}", userAddress.Address.Country.Name }) : null);
    }


    public static List<int> ToListInt(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return new List<int>();
        return str.Split(",").Select(x => int.Parse(x)).ToList();

    }
    public static string GetValueIndex(this LTree? lTree, int index)
    {
        if (!lTree.HasValue)
            return null;
        var splitString = lTree.ToString().Split('.');
        if (splitString.Length - 1 >= index)
            return splitString[index];
        return null;
    }
    public static async Task<TValue> GetOrSetAsync<TKey, TValue>(this Dictionary<TKey, TValue> dictionaries, TKey key, Func<Task<TValue>> acquire)
    {
        if (dictionaries == null)
            dictionaries = new Dictionary<TKey, TValue>();
        if (!dictionaries.ContainsKey(key))
        {
            var value = await acquire();
            dictionaries.Add(key, value);
            return value;

        }
        return dictionaries[key];


    }

    public static DateTime? ToDateTimeWithFormat(this string str, params string[] formats)
    {
        if (DateTime.TryParseExact(str, formats, null, DateTimeStyles.None, out DateTime date))
            return date;
        return null;
    }

    public static string BuildSearchTerm(this string searchText)
    {
        searchText = string.IsNullOrEmpty(searchText) ? string.Empty : searchText;

        var searchTerm = searchText.Trim().Split(" ").Where(x => !string.IsNullOrEmpty(x));
        searchText = string.Join(" ", searchTerm);

        return $"%{searchText}%";
    }

    public static string BuildFullTextSearchTerm(this string keyword)
    {
        keyword = string.IsNullOrEmpty(keyword) ? string.Empty : keyword;

        var keywords = keyword.Trim();

        return $"%{keywords.ToLower()}%";
    }
}
