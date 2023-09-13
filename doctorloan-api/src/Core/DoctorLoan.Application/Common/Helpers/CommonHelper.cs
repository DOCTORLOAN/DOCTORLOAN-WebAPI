using System.ComponentModel;
using System.Globalization;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Domain.Common.Singletons;
using DoctorLoan.Domain.Const.Medias;
using DoctorLoan.Domain.Enums.Commons;
using DoctorLoan.Domain.Enums.Medias;

namespace DoctorLoan.Application.Common.Helpers;
public class CommonHelper
{

    public static object To(object value, Type destinationType)
    {
        return To(value, destinationType, CultureInfo.InvariantCulture);
    }

    public static object To(object value, Type destinationType, CultureInfo culture)
    {
        if (value == null)
            return null;

        var sourceType = value.GetType();

        var destinationConverter = TypeDescriptor.GetConverter(destinationType);
        if (destinationConverter.CanConvertFrom(value.GetType()))
            return destinationConverter.ConvertFrom(null, culture, value);

        var sourceConverter = TypeDescriptor.GetConverter(sourceType);
        if (sourceConverter.CanConvertTo(destinationType))
            return sourceConverter.ConvertTo(null, culture, value, destinationType);

        if (destinationType.IsEnum && value is int)
            return Enum.ToObject(destinationType, (int)value);

        if (!destinationType.IsInstanceOfType(value))
            return Convert.ChangeType(value, destinationType, culture);

        return value;
    }

    public static T To<T>(object value)
    {
        //return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
        return (T)To(value, typeof(T));
    }

    public static List<SelectListItemModel> EnumToSelectListItem<T>() where T : struct, IConvertible
    {
        return Enum.GetValues(typeof(T)).Cast<T>().Select(v => new SelectListItemModel { Text = v.GetDescription(), Value = Convert.ToInt32(v).ToString() }).ToList();
    }
    /// <summary>
    /// Enum to dictionay apply singleton
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Dictionary<T, string> EnumToDictionary<T>(bool loadLanguage = false) where T : struct, IConvertible
    {
        var defaultLanguage = LanguageEnum.VN;
        if (loadLanguage)
            defaultLanguage = EngineContext.GetService<ICurrentUserService>().CurrentLanguage;

        if (Singleton<Dictionary<LanguageEnum, Dictionary<T, string>>>.Instance is null)
            Singleton<Dictionary<LanguageEnum, Dictionary<T, string>>>.Instance = new Dictionary<LanguageEnum, Dictionary<T, string>>();
        if (!Singleton<Dictionary<LanguageEnum, Dictionary<T, string>>>.Instance.ContainsKey(defaultLanguage))
        {
            var function = (T x) => x.GetDescription();
            if (loadLanguage)
            {
                var localizedEntityService = EngineContext.GetService<ILocalizedEntityService>();
                function = (T x) => localizedEntityService.GetLocalizedValueEnumAsync(x, defaultLanguage).Result;
            }
            var listData = Enum.GetValues(typeof(T)).Cast<T>().ToDictionary(x => x, x => function(x));
            Singleton<Dictionary<LanguageEnum, Dictionary<T, string>>>.Instance.Add(defaultLanguage, listData);
        }
        return Singleton<Dictionary<LanguageEnum, Dictionary<T, string>>>.Instance[defaultLanguage];

    }

    public static string GetMediaUrlById(long? mediaId, ImageSize imageSize = ImageSize.Small, bool includeHostName = false)
    {
        string hostName = includeHostName ? EngineContext.GetService<IWebHelper>().GetApiHost().TrimEnd('/') : string.Empty;
        if (!mediaId.HasValue || mediaId == 0)
            return $"{hostName}{MediaConst.NoImageURL}";
        return hostName + String.Format(MediaConst.MediaNoSlugURL, mediaId) + "?size=" + (int)imageSize;
    }

    public static string GetNoImage(bool includeHostName)
    {
        string hostName = includeHostName ? EngineContext.GetService<IWebHelper>().GetApiHost().TrimEnd('/') : string.Empty;
        return $"{hostName}{MediaConst.NoImageURL}";
    }

    public static string GetMediaUrl(long? mediaId, bool? isImage = true, ImageSize? imageSize = null, bool includeHostName = true)
    {
        string hostName = includeHostName ? EngineContext.GetService<IWebHelper>().GetApiHost().TrimEnd('/') : string.Empty;
        var result = hostName;
        if (!mediaId.HasValue || mediaId == 0)
        {
            if (isImage == true)
            {
                result += $"{MediaConst.NoImageURL}";
            }
            else
            {
                result = null;
            }
        }
        else
        {
            result += String.Format(MediaConst.MediaNoSlugURL, mediaId);
            if (imageSize.HasValue)
            {
                result += $"?size={(int)imageSize}";
            }
        }
        return result;
    }
}
