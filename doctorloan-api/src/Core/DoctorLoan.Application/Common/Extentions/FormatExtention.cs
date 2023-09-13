using System.Globalization;
using System.Text;
using DoctorLoan.Domain.Entities.Addresses;
using DoctorLoan.Domain.Enums.Addresses;

namespace DoctorLoan.Application.Common.Extentions;

public static class FormatExtention
{
    public static string FormatUserNameB2C(this string phoneCode, string phone)
    {
        var result = phoneCode.FormatPhoneNo(phone);
        return result?.Replace(" ", "");
    }

    public static string FormatPhoneNo(this string phoneCode, string phone)
    {
        var result = string.Empty;
        if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(phoneCode))
        {
            return result;
        }
        var formatPhone = phone.RemoveFirstZero();

        return phoneCode.Trim().Replace("+", "") + " " + formatPhone;

    }

    public static string DisplayPhoneNo(this string phoneCode, string phone)
    {
        var result = string.Empty;
        if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(phoneCode))
        {
            return result;
        }

        return $"(+{phoneCode.Trim().Replace("+", "")}) {phone.Trim()}";
    }

    public static string DisplayFullName(this string? firstName, string? lastName)
    {
        return $"{firstName?.Trim()} {lastName?.Trim()}".Trim();
    }

    public static string RemoveFirstZero(this string phone)
    {
        var result = string.Empty;
        if (string.IsNullOrEmpty(phone))
        {
            return result;
        }

        var firstPhone = phone[0];
        var formatPhone = phone.Trim();
        if (firstPhone == '0')
        {
            formatPhone = formatPhone[1..];
        }

        return formatPhone;

    }

    public static string TryFormat(this string format, params Object[] args)
    {
        var result = "Invalid";
        try
        {
            result = string.Format(format, args);
            return result;
        }
        catch (FormatException)
        {
            return result;
        }
    }

    public static string DisplayAddress(this Address data)
    {
        var result = string.Empty;
        if (!string.IsNullOrEmpty(data.AddressLine))
        {
            result = data.AddressLine;
        }

        if (data.Ward != null && !string.IsNullOrEmpty(data.Ward.Name))
        {
            result += !string.IsNullOrEmpty(result) ? $", {data.Ward.Name}" : string.Empty;
        }

        if (data.District != null && !string.IsNullOrEmpty(data.District.Name))
        {
            result += !string.IsNullOrEmpty(result) ? $", {data.District.Name}" : string.Empty;
        }

        if (data.Province != null && !string.IsNullOrEmpty(data.Province.Name))
        {
            result += !string.IsNullOrEmpty(result) ? $", {data.Province.Name}" : string.Empty;
        }

        if (data.Country != null && !string.IsNullOrEmpty(data.Country.Name) && data.CountryId != (int)AddressSystemIds.VN)
        {
            result += !string.IsNullOrEmpty(result) ? $", {data.Country.Name}" : string.Empty;
        }

        return result;
    }

    /// <summary>
    /// Creates a URL And SEO friendly slug
    /// </summary>
    /// <param name="text">Text to slugify</param>
    /// <param name="maxLength">Max length of slug</param>
    /// <returns>URL and SEO friendly string</returns>
    public static string UrlFriendly(this string text, int maxLength = 0)
    {
        // Return empty value if text is null
        if (text == null) return "";

        var normalizedString = text
            // Make lowercase
            .ToLowerInvariant()
            // Normalize the text
            .Normalize(NormalizationForm.FormD);

        var stringBuilder = new StringBuilder();
        var stringLength = normalizedString.Length;
        var prevdash = false;
        var trueLength = 0;

        char c;

        for (int i = 0; i < stringLength; i++)
        {
            c = normalizedString[i];

            switch (CharUnicodeInfo.GetUnicodeCategory(c))
            {
                // Check if the character is a letter or a digit if the character is a
                // international character remap it to an ascii valid character
                case UnicodeCategory.LowercaseLetter:
                case UnicodeCategory.UppercaseLetter:
                case UnicodeCategory.DecimalDigitNumber:
                    if (c < 128)
                        stringBuilder.Append(c);
                    else
                        stringBuilder.Append(RemapInternationalCharToAscii(c));

                    prevdash = false;
                    trueLength = stringBuilder.Length;
                    break;

                // Check if the character is to be replaced by a hyphen but only if the last character wasn't
                case UnicodeCategory.SpaceSeparator:
                case UnicodeCategory.ConnectorPunctuation:
                case UnicodeCategory.DashPunctuation:
                case UnicodeCategory.OtherPunctuation:
                case UnicodeCategory.MathSymbol:
                    if (!prevdash)
                    {
                        stringBuilder.Append('-');
                        prevdash = true;
                        trueLength = stringBuilder.Length;
                    }
                    break;
            }

            // If we are at max length, stop parsing
            if (maxLength > 0 && trueLength >= maxLength)
                break;
        }

        // Trim excess hyphens
        var result = stringBuilder.ToString().Trim('-');

        // Remove any excess character to meet maxlength criteria
        return maxLength <= 0 || result.Length <= maxLength ? result : result.Substring(0, maxLength);
    }

    /// <summary>
    /// Remaps international characters to ascii compatible ones
    /// based of: https://meta.stackexchange.com/questions/7435/non-us-ascii-characters-dropped-from-full-profile-url/7696#7696
    /// </summary>
    /// <param name="c">Charcter to remap</param>
    /// <returns>Remapped character</returns>
    public static string RemapInternationalCharToAscii(char c)
    {
        string s = c.ToString().ToLowerInvariant();
        if ("àåáâäãåą".Contains(s))
        {
            return "a";
        }
        else if ("èéêëę".Contains(s))
        {
            return "e";
        }
        else if ("ìíîïı".Contains(s))
        {
            return "i";
        }
        else if ("òóôõöøőð".Contains(s))
        {
            return "o";
        }
        else if ("ùúûüŭů".Contains(s))
        {
            return "u";
        }
        else if ("çćčĉ".Contains(s))
        {
            return "c";
        }
        else if ("żźž".Contains(s))
        {
            return "z";
        }
        else if ("śşšŝ".Contains(s))
        {
            return "s";
        }
        else if ("ñń".Contains(s))
        {
            return "n";
        }
        else if ("ýÿ".Contains(s))
        {
            return "y";
        }
        else if ("ğĝ".Contains(s))
        {
            return "g";
        }
        else if (c == 'ř')
        {
            return "r";
        }
        else if (c == 'ł')
        {
            return "l";
        }
        else if (c == 'đ')
        {
            return "d";
        }
        else if (c == 'ß')
        {
            return "ss";
        }
        else if (c == 'þ')
        {
            return "th";
        }
        else if (c == 'ĥ')
        {
            return "h";
        }
        else if (c == 'ĵ')
        {
            return "j";
        }
        else
        {
            return "";
        }
    }

    public static string FormatCurrency(this decimal money)
    {
        return money.ToString("#,###");
    }

    public static string FormatNumber(this int number)
    {
        return number.ToString("#,###");
    }

    private static readonly string[] VietnameseSigns = new string[]
        {

            "aAeEoOuUiIdDyY",

            "áàạảãâấầậẩẫăắằặẳẵ",

            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",

            "éèẹẻẽêếềệểễ",

            "ÉÈẸẺẼÊẾỀỆỂỄ",

            "óòọỏõôốồộổỗơớờợởỡ",

            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",

            "úùụủũưứừựửữ",

            "ÚÙỤỦŨƯỨỪỰỬỮ",

            "íìịỉĩ",

            "ÍÌỊỈĨ",

            "đ",

            "Đ",

            "ýỳỵỷỹ",

            "ÝỲỴỶỸ"
        };

    public static string RemoveSign4VietnameseString(this string str)
    {
        if (string.IsNullOrEmpty(str)) return string.Empty;
        for (int i = 1; i < VietnameseSigns.Length; i++)
        {
            for (int j = 0; j < VietnameseSigns[i].Length; j++)
                str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
        }
        return str;
    }
    public static string ToSlug(this string str)
    {
        if (string.IsNullOrEmpty(str)) return string.Empty;

        return str.RemoveSign4VietnameseString().Replace(" ", "-").ToLower();
    }

}

