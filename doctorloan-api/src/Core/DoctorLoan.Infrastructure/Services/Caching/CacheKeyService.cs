using System.Globalization;
using System.Text;
using DoctorLoan.Application.Common.Helpers;
using DoctorLoan.Application.Models.Settings;
using DoctorLoan.Domain.Common.Caching;

namespace DoctorLoan.Infrastructure.Services.Caching;

public abstract class CacheKeyService
{
    #region Constants


    private HashAlgorithmType HashAlgorithm => HashAlgorithmType.SHA1;

    #endregion

    #region Fields

    protected readonly AppConfiguration _appConfiguration;

    #endregion

    #region Ctor

    protected CacheKeyService(AppConfiguration appConfiguration)
    {
        _appConfiguration = appConfiguration;
    }

    #endregion

    #region Utilities


    protected virtual string PrepareKeyPrefix(string prefix, params object[] prefixParameters)
    {
        return prefixParameters?.Any() ?? false
            ? string.Format(prefix, prefixParameters.Select(CreateCacheKeyParameters).ToArray())
            : prefix;
    }


    protected virtual string CreateIdsHash(IEnumerable<int> ids)
    {
        var identifiers = ids.ToList();

        if (!identifiers.Any())
            return string.Empty;

        var identifiersString = string.Join(", ", identifiers.OrderBy(id => id));
        return HashHelper.CreateHash(Encoding.UTF8.GetBytes(identifiersString), HashAlgorithm);
    }


    protected virtual object CreateCacheKeyParameters(object parameter)
    {
        return parameter switch
        {
            null => "null",
            IEnumerable<int> ids => CreateIdsHash(ids),
            IEnumerable<BaseEntity<int>> entities => CreateIdsHash(entities.Select(entity => entity.Id)),
            IBaseEntity<int> entity => entity.Id,
            decimal param => param.ToString(CultureInfo.InvariantCulture),
            _ => parameter
        };
    }

    #endregion

    #region Methods


    public virtual CacheKey PrepareKey(CacheKey cacheKey, params object[] cacheKeyParameters)
    {
        return cacheKey.Create(CreateCacheKeyParameters, cacheKeyParameters);
    }


    public virtual CacheKey PrepareKeyForDefaultCache(CacheKey cacheKey, params object[] cacheKeyParameters)
    {
        var key = cacheKey.Create(CreateCacheKeyParameters, cacheKeyParameters);

        key.CacheTime = _appConfiguration.DefaultCacheTime;

        return key;
    }


    public virtual CacheKey PrepareKeyForShortTermCache(CacheKey cacheKey, params object[] cacheKeyParameters)
    {
        var key = cacheKey.Create(CreateCacheKeyParameters, cacheKeyParameters);

        key.CacheTime = _appConfiguration.ShortTermCacheTime;

        return key;
    }

    #endregion
}
