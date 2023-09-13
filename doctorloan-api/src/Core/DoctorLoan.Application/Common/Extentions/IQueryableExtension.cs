using System.Linq.Expressions;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Domain.Common;
using DoctorLoan.Domain.Enums.Commons;
using DoctorLoan.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DoctorLoan.Application.Common.Extentions;
public static class IQueryableExtension
{
    public static Task<PaginatedList<T>> ToPagedListAsync<T>(
       this IQueryable<T> source,
       QueryParam query,
       CancellationToken cancellationToken = default)
    {
        return source.ToPagedListAsync(query.Page, query.Take, cancellationToken);
    }
    public static async Task<PaginatedList<T>> ToPagedListAsync<T>(
        this IQueryable<T> source,
        int page,
        int take,
        CancellationToken cancellationToken = default)
    {
        page = Math.Max(page, 1);
        var skip = (page - 1) * take;
        take = take < 0 ? int.MaxValue : take;
        var total = await source.CountAsync(cancellationToken);
        if (total == 0)
            return new PaginatedList<T>(new List<T>(), total, page, take);
        var items = await source.Skip(skip).Take(take).ToListAsync(cancellationToken);

        return new PaginatedList<T>(items, total, page, take);
    }

    public static async Task<List<T>> ToPagedListWithoutTotalAsync<T>(
        this IQueryable<T> source,
        int page,
        int take,
        CancellationToken cancellationToken = default)
    {
        page = Math.Max(page, 1);
        var skip = (page - 1) * take;
        take = take < 0 ? int.MaxValue : take;

        var items = await source.Skip(skip).Take(take).ToListAsync(cancellationToken);

        return new List<T>(items);
    }

    public static IOrderedQueryable<T> Sort<T>(this IQueryable<T> query, Expression<Func<T, object>> orderBy, bool asc = true)
    {
        return asc ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
    }

    public static IQueryable<TEntity> QueryTable<TEntity>(this IQueryable<TEntity> query, string[] includeProperties = null) where TEntity : class
    {
        if (includeProperties != null)
        {
            foreach (var navigationExpr in includeProperties)
            {
                query = query.Include(navigationExpr);
            }
        }

        return query;
    }

    public static Task<TEntity> GetByIdAsync<TEntity, TIdType>(this DbSet<TEntity> dbSet, TIdType id, CancellationToken cancellation = default) where TEntity : BaseEntityAudit<TIdType>
    {
        return dbSet.FirstOrDefaultAsync(x => x.Id.Equals(id), cancellation);
    }

    public static IQueryable<TEntity> FilterStatus<TEntity>(this IQueryable<TEntity> query, BaseEntityStatus? status) where TEntity : IBaseActiveStatus
    {
        return !status.HasValue || status == 0 ? query : query.Where(x => x.Status == status);
    }
    public static Task<List<SelectListItemModel>> ToSelectListModelAsync<TSource, TValue, TText>(this IQueryable<TSource> source,
        Func<TSource, TValue> valueSelector, Func<TSource, TText> textSelector,
        CancellationToken cancellationToken = default)
    {
        return source.Select(x => new SelectListItemModel
        {
            Value = valueSelector(x).ToString(),
            Text = textSelector(x).ToString()
        }).ToListAsync(cancellationToken);
    }
}
