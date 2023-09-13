using DoctorLoan.Domain.Common.Caching;
using DoctorLoan.Domain.Entities.Settings;

namespace DoctorLoan.Domain.Const.Settings;
public class SettingsCacheKeys
{
    public static CacheKey SettingCachingKey = new("DoctorLoan.setting.all.dictionary.", DoctorLoanEntityCacheDefaults<Setting>.Prefix);
}
