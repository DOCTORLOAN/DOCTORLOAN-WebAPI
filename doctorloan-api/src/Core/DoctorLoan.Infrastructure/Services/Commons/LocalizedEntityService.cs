
using System.Linq.Expressions;
using System.Reflection;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Common.Helpers;
using DoctorLoan.Application.Interfaces.Caching;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Domain.Const.Settings;
using DoctorLoan.Domain.Entities.Commons;
using DoctorLoan.Domain.Enums.Commons;
using DoctorLoan.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace DoctorLoan.Infrastructure.Services.Commons;

public partial class LocalizedEntityService : ILocalizedEntityService
{
    #region Fields

    private readonly IApplicationDbContext _context;
    private readonly IStaticCacheManager _staticCacheManager;

    #endregion

    #region Ctor

    public LocalizedEntityService(IApplicationDbContext context,
    IStaticCacheManager staticCacheManager)
    {
        _context = context;
        _staticCacheManager = staticCacheManager;

    }

    #endregion  

    #region Methods
    public async Task<TPropType> GetLocalizedAsync<TEntity, TPropType>(TEntity entity, Expression<Func<TEntity, TPropType>> keySelector,
            LanguageEnum? language = null)
            where TEntity : IBaseEntity<int>, ILocalizedEntity
    {
        if (entity == null)
            return default(TPropType);

        if (keySelector.Body is not MemberExpression member)
            throw new ArgumentException($"Expression '{keySelector}' refers to a method, not a property.");

        if (member.Member is not PropertyInfo propInfo)
            throw new ArgumentException($"Expression '{keySelector}' refers to a field, not a property.");

        var result = default(TPropType);
        var resultStr = string.Empty;

        var localeKeyGroup = entity.GetType().Name;
        var localeKey = propInfo.Name;


        if (!language.HasValue)
            language = LanguageEnum.VN;
        //localized value
        resultStr = await GetLocalizedValueAsync((int)language, Convert.ToInt32(entity.Id), localeKeyGroup, localeKey);
        if (!string.IsNullOrEmpty(resultStr))
            result = CommonHelper.To<TPropType>(resultStr);
        if (!string.IsNullOrEmpty(resultStr))
            return result;
        var localizer = keySelector.Compile();
        result = localizer(entity);
        return result;
    }

    public async Task<string> GetLocalizedValueAsync(int languageId, int entityId, string localeKeyGroup, string localeKey)
    {
        var key = _staticCacheManager.PrepareKeyForDefaultCache(LocalizationCacheKeys.LocalizedPropertyCacheKey
            , languageId, entityId, localeKeyGroup, localeKey);

        return await _staticCacheManager.GetAsync(key, async () =>
        {

            var query = from lp in _context.LocalizedProperties
                        where lp.LanguageId == languageId &&
                              lp.EntityId == entityId &&
                              lp.LocaleKeyGroup == localeKeyGroup &&
                              lp.LocaleKey == localeKey
                        select lp.LocaleValue;

            //little hack here. nulls aren't cacheable so set it to ""
            var localeValue = (await query.FirstOrDefaultAsync()) ?? string.Empty;

            return localeValue;
        });
    }

    public async Task<string> GetLocalizedValueEnumAsync<TEnum>(TEnum @enum, LanguageEnum language = LanguageEnum.VN) where TEnum : struct, IConvertible
    {
        string localeKeyGroup = typeof(TEnum).FullName;
        string localeKey = @enum.ToString();
        int id = (int)Convert.ChangeType(@enum, typeof(int));
        var value = await GetLocalizedValueAsync((int)language, id, localeKeyGroup, localeKey);
        if (string.IsNullOrEmpty(value))
            return @enum.GetDescription();
        return value;


    }

    public async Task SaveLocalizedValueAsync<T>(T entity,
        Expression<Func<T, string>> keySelector,
        string localeValue,
        int languageId) where T : IBaseEntity<int>, ILocalizedEntity
    {
        await SaveLocalizedValueAsync<T, string>(entity, keySelector, localeValue, languageId);
    }

    public virtual async Task SaveLocalizedValueAsync<T, TPropType>(T entity,
        Expression<Func<T, TPropType>> keySelector,
        TPropType localeValue,
        int languageId) where T : IBaseEntity<int>, ILocalizedEntity
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        if (languageId == 0)
            throw new ArgumentOutOfRangeException(nameof(languageId), "Language ID should not be 0");

        if (keySelector.Body is not MemberExpression member)
        {
            throw new ArgumentException(string.Format(
                "Expression '{0}' refers to a method, not a property.",
                keySelector));
        }

        var propInfo = member.Member as PropertyInfo;
        if (propInfo == null)
        {
            throw new ArgumentException(string.Format(
                   "Expression '{0}' refers to a field, not a property.",
                   keySelector));
        }

        //load localized value (check whether it's a cacheable entity. In such cases we load its original entity type)
        var localeKeyGroup = entity.GetType().Name;
        var localeKey = propInfo.Name;
        var localeValueStr = CommonHelper.To<string>(localeValue);
        await SaveLocalizedPropertiesAsync(entity.Id, localeKeyGroup, languageId, localeKey, localeValueStr);
    }
    public async Task SaveLocalizedPropertiesAsync(int id, string localeKeyGroup, int languageId, string localeKey, string localeValueStr)
    {

        var props = await GetLocalizedPropertiesAsync(id, localeKeyGroup);
        var prop = props.FirstOrDefault(lp => lp.LanguageId == languageId &&
            lp.LocaleKey.Equals(localeKey, StringComparison.InvariantCultureIgnoreCase)); //should be culture invariant



        if (prop != null)
        {
            if (string.IsNullOrWhiteSpace(localeValueStr))
            {
                //delete
                _context.LocalizedProperties.Remove(prop);
            }
            else
            {
                //update
                prop.LocaleValue = localeValueStr;

            }
            await _context.SaveChangesAsync(CancellationToken.None);
        }
        else
        {
            if (string.IsNullOrWhiteSpace(localeValueStr))
                return;

            //insert
            prop = new LocalizedProperty
            {
                EntityId = id,
                LanguageId = languageId,
                LocaleKey = localeKey,
                LocaleKeyGroup = localeKeyGroup,
                LocaleValue = localeValueStr
            };
            await _context.LocalizedProperties.AddAsync(prop);
            await _context.SaveChangesAsync(CancellationToken.None);
        }
    }

    #endregion

    #region Utilities

    protected virtual async Task<IList<LocalizedProperty>> GetLocalizedPropertiesAsync(int entityId, string localeKeyGroup)
    {
        if (entityId == 0 || string.IsNullOrEmpty(localeKeyGroup))
            return new List<LocalizedProperty>();

        var query = from lp in _context.LocalizedProperties
                    orderby lp.Id
                    where lp.EntityId == entityId &&
                          lp.LocaleKeyGroup == localeKeyGroup
                    select lp;

        var props = await query.ToListAsync();
        return props;
    }

    protected virtual async Task<IList<LocalizedProperty>> GetAllLocalizedPropertiesAsync()
    {
        var key = _staticCacheManager.PrepareKeyForDefaultCache(LocalizationCacheKeys.LocalizedPropertyAllCacheKey);
        return await _staticCacheManager.GetAsync(key, async () => await _context.LocalizedProperties.ToListAsync());

    }





    #endregion
}