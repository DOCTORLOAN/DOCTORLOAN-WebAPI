namespace DoctorLoan.Domain.Common.Caching;

/// <summary>
/// Represents key for caching objects
/// </summary>
public partial class CacheKey
{
    #region Ctor

    /// <summary>
    /// Initialize a new instance with key and prefixes
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="prefixes">Prefixes for remove by prefix functionality</param>
    public CacheKey(string key, params string[] prefixes)
    {
        Key = key;
        Prefixes.AddRange(prefixes.Where(prefix => !string.IsNullOrEmpty(prefix)));
    }

    #endregion

    #region Methods
    public virtual CacheKey Create(Func<object, object> createCacheKeyParameters, params object[] keyObjects)
    {
        var cacheKey = new CacheKey(Key, Prefixes.ToArray());

        if (!keyObjects.Any())
            return cacheKey;

        cacheKey.Key = string.Format(cacheKey.Key, keyObjects.Select(createCacheKeyParameters).ToArray());

        for (var i = 0; i < cacheKey.Prefixes.Count; i++)
            cacheKey.Prefixes[i] = string.Format(cacheKey.Prefixes[i], keyObjects.Select(createCacheKeyParameters).ToArray());

        return cacheKey;
    }

    #endregion

    #region Properties


    public string Key { get; protected set; }


    public List<string> Prefixes { get; protected set; } = new List<string>();


    public int CacheTime { get; set; } = 60;

    #endregion
}
