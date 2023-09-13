using System.Collections.Concurrent;
using DoctorLoan.Application.Interfaces.Caching;
using DoctorLoan.Application.Models.Settings;
using DoctorLoan.Domain.Common.Caching;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace DoctorLoan.Infrastructure.Services.Caching;

public partial class MemoryCacheManager : CacheKeyService, ILocker, IStaticCacheManager
{
    #region Fields

    // Flag: Has Dispose already been called?
    private bool _disposed;

    private readonly IMemoryCache _memoryCache;

    private static readonly ConcurrentDictionary<string, CancellationTokenSource> _prefixes = new();
    private static CancellationTokenSource _clearToken = new();

    #endregion

    #region Ctor

    public MemoryCacheManager(IOptions<AppConfiguration> appConfiguration, IMemoryCache memoryCache) : base(appConfiguration.Value)
    {
        _memoryCache = memoryCache;
    }
    #endregion 

    #region Methods
    public Task RemoveAsync(CacheKey cacheKey, params object[] cacheKeyParameters)
    {
        Remove(cacheKey, cacheKeyParameters);
        return Task.CompletedTask;
    }

    public async Task<T> GetAsync<T>(CacheKey key, Func<Task<T>> acquire)
    {
        if ((key?.CacheTime ?? 0) <= 0)
            return await acquire();

        if (_memoryCache.TryGetValue(key.Key, out T result))
            return result;

        result = await acquire();

        if (result != null)
            await SetAsync(key, result);

        return result;
    }


    public async Task<T> GetAsync<T>(CacheKey key, Func<T> acquire)
    {
        if ((key?.CacheTime ?? 0) <= 0)
            return acquire();

        var result = _memoryCache.GetOrCreate(key.Key, entry =>
        {
            entry.SetOptions(PrepareEntryOptions(key));

            return acquire();
        });

        //do not cache null value
        if (result == null)
            await RemoveAsync(key);

        return result;
    }


    public T Get<T>(CacheKey key, Func<T> acquire)
    {
        if ((key?.CacheTime ?? 0) <= 0)
            return acquire();

        if (_memoryCache.TryGetValue(key.Key, out T result))
            return result;

        result = acquire();

        if (result != null)
            Set(key, result);

        return result;
    }


    public Task SetAsync(CacheKey key, object data)
    {
        Set(key, data);

        return Task.CompletedTask;
    }

    public bool PerformActionWithLock(string key, TimeSpan expirationTime, Action action)
    {
        //ensure that lock is acquired
        if (_memoryCache.TryGetValue(key, out _))
            return false;

        try
        {
            _memoryCache.Set(key, key, expirationTime);

            //perform action
            action();

            return true;
        }
        finally
        {
            //release lock even if action fails
            _memoryCache.Remove(key);
        }
    }


    public Task RemoveByPrefixAsync(string prefix, params object[] prefixParameters)
    {
        prefix = PrepareKeyPrefix(prefix, prefixParameters);

        _prefixes.TryRemove(prefix, out var tokenSource);
        tokenSource?.Cancel();
        tokenSource?.Dispose();

        return Task.CompletedTask;
    }


    public Task ClearAsync()
    {
        _clearToken.Cancel();
        _clearToken.Dispose();

        _clearToken = new CancellationTokenSource();

        foreach (var prefix in _prefixes.Keys.ToList())
        {
            _prefixes.TryRemove(prefix, out var tokenSource);
            tokenSource?.Dispose();
        }

        return Task.CompletedTask;
    }


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }


    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
            _memoryCache.Dispose();

        _disposed = true;
    }

    #endregion

    #region Utilities


    private MemoryCacheEntryOptions PrepareEntryOptions(CacheKey key)
    {
        //set expiration time for the passed cache key
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(key.CacheTime)
        };

        //add tokens to clear cache entries
        options.AddExpirationToken(new CancellationChangeToken(_clearToken.Token));
        foreach (var keyPrefix in key.Prefixes.ToList())
        {
            var tokenSource = _prefixes.GetOrAdd(keyPrefix, new CancellationTokenSource());
            options.AddExpirationToken(new CancellationChangeToken(tokenSource.Token));
        }

        return options;
    }


    private void Remove(CacheKey cacheKey, params object[] cacheKeyParameters)
    {
        cacheKey = PrepareKey(cacheKey, cacheKeyParameters);
        _memoryCache.Remove(cacheKey.Key);
    }


    private void Set(CacheKey key, object data)
    {
        if ((key?.CacheTime ?? 0) <= 0 || data == null)
            return;

        _memoryCache.Set(key.Key, data, PrepareEntryOptions(key));
    }

    #endregion
}
