using System.Linq.Expressions;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Domain.Entities.Contents;
using Microsoft.EntityFrameworkCore;

namespace DoctorLoan.Contents.Application.Common.Expressions;
public static class ContentExpression
{
    public static Expression<Func<Content, bool>> IsContains(string keyword)
    {
        var search = keyword.BuildFullTextSearchTerm();

        Expression<Func<Content, bool>> expression = x => EF.Functions.Like(x.Code.ToLower(), search)
                                                || EF.Functions.Like(x.Name.ToLower(), search);

        return expression;
    }
}
