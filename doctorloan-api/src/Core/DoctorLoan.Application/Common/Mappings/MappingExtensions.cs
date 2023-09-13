using DoctorLoan.Application.Models.Commons;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace DoctorLoan.Application.Common.Mappings;

public static class MappingExtensions
{
    public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize) where TDestination : class
        => PaginatedList<TDestination>.CreateAsync(queryable.AsNoTracking(), pageNumber, pageSize);

    public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable) where TDestination : class
        => EngineContext.GetService<IMapper>().ProjectTo<TDestination>(queryable).ToListAsync();
}
