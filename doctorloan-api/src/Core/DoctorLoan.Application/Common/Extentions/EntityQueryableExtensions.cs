using System.Linq.Expressions;
using DoctorLoan.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace DoctorLoan.Application.Common.Extentions;
public static class EntityQueryableExtensions
{
    #region Orders

    public static IQueryable<T> AddFilter<T>(IQueryable<T> query, string propertyName, string searchTerm)
    {
        var param = Expression.Parameter(typeof(T), "e");
        var propExpression = Expression.Property(param, propertyName);

        object value = searchTerm;
        if (propExpression.Type != typeof(string))
            value = Convert.ChangeType(value, propExpression.Type);

        var filterLambda = Expression.Lambda<Func<T, bool>>(
            Expression.Equal(
                propExpression,
                Expression.Constant(value)
            ),
            param
        );

        return query.Where(filterLambda);
    }
    #endregion

    #region Users
    public static IQueryable<User> IncludeUserAddress(this IQueryable<User> users)
    {
        return users.Include(x => x.UserAddresses).ThenInclude(x => x.Address).ThenInclude(x => x.Ward).ThenInclude(x => x.District).ThenInclude(x => x.Province);
    }

    #endregion
}
