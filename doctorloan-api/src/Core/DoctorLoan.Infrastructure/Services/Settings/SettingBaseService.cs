using System.ComponentModel;
using DoctorLoan.Application.Common.Helpers;
using DoctorLoan.Application.Interfaces.Caching;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Interfaces.Settings;
using DoctorLoan.Domain.Common.Caching;
using DoctorLoan.Domain.Const.Settings;
using DoctorLoan.Domain.Entities.Settings;
using DoctorLoan.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DoctorLoan.Infrastructure.Services.Settings;
public class SettingBaseService : ISettingBaseService
{
    #region Fields
    private readonly IApplicationDbContext _dbContext;
    private readonly IStaticCacheManager _staticCacheManager;
    #endregion

    #region Ctor
    public SettingBaseService(IApplicationDbContext dbContext, IStaticCacheManager staticCacheManager)
    {
        _dbContext = dbContext;
        _staticCacheManager = staticCacheManager;
    }
    #endregion

    #region Methods
    public async Task<ISettings> LoadSettingAsync(Type type, int systemId = 0)
    {
        var settings = Activator.CreateInstance(type);

        //if (!DataSettingsManager.IsDatabaseInstalled())
        //    return settings as ISettings;

        foreach (var prop in type.GetProperties())
        {
            // get properties we can read and write to
            if (!prop.CanRead || !prop.CanWrite)
                continue;

            var key = type.Name + "." + prop.Name;
            //load by store
            var setting = await GetSettingByKeyAsync<string>(key, systemId: systemId, loadSharedValueIfNotFound: true);
            if (setting == null)
                continue;

            if (!TypeDescriptor.GetConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
                continue;

            if (!TypeDescriptor.GetConverter(prop.PropertyType).IsValid(setting))
                continue;

            var value = TypeDescriptor.GetConverter(prop.PropertyType).ConvertFromInvariantString(setting);

            //set property
            prop.SetValue(settings, value, null);
        }

        return settings as ISettings;
    }
    public async Task<T> GetSettingByKeyAsync<T>(string key, T defaultValue = default,
            int systemId = 0, bool loadSharedValueIfNotFound = false)
    {
        if (string.IsNullOrEmpty(key))
            return defaultValue;

        var settings = await GetAllSettingsDictionaryAsync();
        key = key.Trim().ToLowerInvariant();
        if (!settings.ContainsKey(key))
            return defaultValue;

        var settingsByKey = settings[key];
        var setting = settingsByKey.FirstOrDefault(x => x.SystemId == systemId);

        //load shared value?
        if (setting == null && systemId > 0 && loadSharedValueIfNotFound)
            setting = settingsByKey.FirstOrDefault(x => x.SystemId == 0);

        return setting != null ? CommonHelper.To<T>(setting.Value) : defaultValue;
    }

    public async Task SaveSettingAsync<T>(T settings, int systemId = 0) where T : ISettings, new()
    {
        /* We do not clear cache after each setting update.
         * This behavior can increase performance because cached settings will not be cleared 
         * and loaded from database after each update */
        foreach (var prop in typeof(T).GetProperties())
        {
            // get properties we can read and write to
            if (!prop.CanRead || !prop.CanWrite)
                continue;

            if (!TypeDescriptor.GetConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
                continue;

            var key = typeof(T).Name + "." + prop.Name;
            var value = prop.GetValue(settings, null);
            if (value != null)
                await SetSettingAsync(prop.PropertyType, key, value, systemId, false);
            else
                await SetSettingAsync(key, string.Empty, systemId, false);
        }

        //and now clear cache

    }
    public async Task SetSettingAsync<T>(string key, T value, int systemId = 0, bool clearCache = true)
    {
        await SetSettingAsync(typeof(T), key, value, systemId, clearCache);
    }
    #endregion

    #region Privates
    protected async Task<IDictionary<string, IList<Setting>>> GetAllSettingsDictionaryAsync()
    {
        return await _staticCacheManager.GetAsync(SettingsCacheKeys.SettingCachingKey, async () =>
        {
            var settings = await _dbContext.Settings.ToListAsync();

            var dictionary = new Dictionary<string, IList<Setting>>();
            foreach (var s in settings)
            {
                var resourceName = s.Name.ToLowerInvariant();
                var settingForCaching = new Setting
                {
                    Id = s.Id,
                    Name = s.Name,
                    Value = s.Value,
                    SettingApp = s.SettingApp
                };
                if (!dictionary.ContainsKey(resourceName))
                    //first setting
                    dictionary.Add(resourceName, new List<Setting>
                        {
                            settingForCaching
                        });
                else
                    //already added
                    //most probably it's the setting with the same name but for some certain store (storeId > 0)
                    dictionary[resourceName].Add(settingForCaching);
            }

            return dictionary;
        });
    }
    private async Task SetSettingAsync(Type type, string key, object value, int systemId = 0, bool clearCache = false)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));
        key = key.Trim().ToLowerInvariant();
        var valueStr = TypeDescriptor.GetConverter(type).ConvertToInvariantString(value);


        var setting = _dbContext.Settings.FirstOrDefault(x => x.SystemId == systemId);
        if (setting != null)
        {
            //update

            setting.Value = valueStr;
        }
        else
        {
            //insert
            setting = new Setting
            {
                Name = key,
                Value = valueStr,
                SystemId = systemId
            };
            await _dbContext.Settings.AddAsync(setting);
        }

        await _dbContext.SaveChangesAsync(CancellationToken.None);
        if (clearCache)
            await _staticCacheManager.RemoveByPrefixAsync(DoctorLoanEntityCacheDefaults<Setting>.Prefix);
    }
    private async Task ClearCache()
    {
        await _staticCacheManager.RemoveByPrefixAsync(DoctorLoanEntityCacheDefaults<Setting>.Prefix);
    }
    #endregion
}
