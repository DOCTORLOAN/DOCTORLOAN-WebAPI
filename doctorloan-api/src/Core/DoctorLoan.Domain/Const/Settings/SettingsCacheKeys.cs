namespace DoctorLoan.Domain.Common.Caching;

/// <summary>
/// Provides common cache key prefixes for entity-based cache keys.
/// </summary>
public static partial class DoctorLoanEntityCacheDefaults<TEntity>
{
    /// <summary>
    /// Root prefix for all entity cache keys.
    /// </summary>
    public static string Prefix => "doctorloan.entity";

    /// <summary>
    /// Prefix for cache keys that retrieve entities by identifier.
    /// </summary>
    public static string ByIdPrefix => "doctorloan.entity.by-id";
}
