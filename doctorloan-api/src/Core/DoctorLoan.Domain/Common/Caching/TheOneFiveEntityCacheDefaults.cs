namespace DoctorLoan.Domain.Common.Caching;

/// <summary>
/// Provides cache key defaults for a specific entity type.
/// </summary>
/// <typeparam name="TEntity">
/// The entity type that implements <see cref="IBaseEntity{TKey}"/>.
/// </typeparam>
public static partial class DoctorLoanEntityCacheDefaults<TEntity>
    where TEntity : IBaseEntity<int>, new()
{
    /// <summary>
    /// Gets the normalized entity type name used in cache keys.
    /// </summary>
    public static string EntityTypeName =>
        typeof(TEntity).Name.ToLowerInvariant();

    /// <summary>
    /// Cache key used to retrieve an entity instance by its identifier.
    /// </summary>
    public static CacheKey ByIdCacheKey =>
        new CacheKey(
            $"doctorloan.{EntityTypeName}.by-id.{{0}}",
            ByIdPrefix,
            Prefix
        );
}
