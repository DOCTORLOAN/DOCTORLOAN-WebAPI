using System.Linq.Expressions;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Domain.Entities.Customers;
using Microsoft.EntityFrameworkCore;

namespace DoctorLoan.Customer.Application.Commons.Expressions;

public static class CustomerExpression
{
    public static Expression<Func<Domain.Entities.Customers.Customer, bool>> GetCustomerByPhone(string phone) => x =>
          x.Phone == phone;

    public static Expression<Func<Domain.Entities.Customers.Customer, bool>> GetCustomerByEmail(string email) => x =>
         x.Email.ToLower() == email.ToLower();

    public static Expression<Func<Domain.Entities.Customers.Customer, bool>> Exist(string? phone, string? email) => x =>
          x.Phone == phone || x.Email.ToLower() == email.ToLower();

    public static Expression<Func<Domain.Entities.Customers.Customer, bool>> IsContains(string keyword)
    {
        var search = keyword.BuildFullTextSearchTerm();

        Expression<Func<Domain.Entities.Customers.Customer, bool>> expression = x => EF.Functions.Like(x.FullName.ToLower(), search)
                                                        || EF.Functions.Like(x.Email.ToLower(), search)
                                                        || EF.Functions.Like(("0" + x.Phone), search);

        return expression;
    }

    public static Expression<Func<CustomerAddress, bool>> AddressContain(string keyword)
    {
        var search = keyword.BuildFullTextSearchTerm();

        Expression<Func<CustomerAddress, bool>> expression = x => EF.Functions.Like(x.FullName.ToLower(), search)
                                                        || EF.Functions.Like(("0" + x.Phone), search);

        return expression;
    }
}
