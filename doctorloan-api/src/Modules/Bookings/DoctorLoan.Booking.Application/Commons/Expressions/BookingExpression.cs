using System.Linq.Expressions;
using DoctorLoan.Application.Common.Extentions;
using Microsoft.EntityFrameworkCore;

namespace DoctorLoan.Booking.Application.Commons.Expressions;

public static class BookingExpression
{
    public static Expression<Func<Domain.Entities.Bookings.Booking, bool>> GetByPhone(string phone) => x =>
          x.Customer.Phone == phone;

    public static Expression<Func<Domain.Entities.Bookings.Booking, bool>> IsContains(string keyword)
    {
        var search = keyword.BuildFullTextSearchTerm();

        Expression<Func<Domain.Entities.Bookings.Booking, bool>> expression = x => EF.Functions.Like(x.Customer.FullName.ToLower(), search)
                                                        || EF.Functions.Like(("0" + x.Customer.Phone), search);

        return expression;
    }
}
