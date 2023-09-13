using System.Linq.Expressions;
using DoctorLoan.Application.Common.Extentions;
using Microsoft.EntityFrameworkCore;

namespace DoctorLoan.Order.Application.Commons.Expressions;
public static class OrderExpression
{
    public static Expression<Func<Domain.Entities.Orders.Order, bool>> IsContains(string keyword)
    {
        var search = keyword.BuildFullTextSearchTerm();

        Expression<Func<Domain.Entities.Orders.Order, bool>> expression = x =>
                                                            EF.Functions.Like(x.OrderNo.ToLower(), search)
                                                        || EF.Functions.Like(x.FullName.ToLower(), search)
                                                        || EF.Functions.Like(("0" + x.Phone), search)
                                                        || EF.Functions.Like(x.Email.ToLower(), search);

        return expression;
    }
}
