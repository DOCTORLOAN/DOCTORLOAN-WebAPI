using DoctorLoan.Domain.Common.Caching;

namespace DoctorLoan.Domain.Const.Settings;

public static class LocalizationCacheKeys
{
    public static CacheKey LocalizedPropertyAllCacheKey => new("DoctorLoan.localizedproperties");
    public static CacheKey LocalizedPropertyCacheKey => new("DoctorLoan.localizedproperty.value.{0}-{1}-{2}-{3}");
}