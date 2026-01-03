using System.Linq.Expressions;
using DoctorLoan.Application.Common.Extentions;
using Microsoft.EntityFrameworkCore;

namespace DoctorLoan.MedicalRecord.Application.Commons.Expressions;
public static class MedicalRecordExpression
{
    public static Expression<Func<Domain.Entities.MedicalRecord.MedicalRecord, bool>> GetByPhone(string phone) =>
          x => x.Customer.Phone == phone;

    public static Expression<Func<Domain.Entities.MedicalRecord.MedicalRecord, bool>> IsContains(string keyword)
    {
        var search = keyword.BuildFullTextSearchTerm();

        Expression<Func<Domain.Entities.MedicalRecord.MedicalRecord, bool>> expression = x => EF.Functions.Like(x.Customer.FullName.ToLower(), search)
                                                        || EF.Functions.Like(("0" + x.Customer.Phone), search);

        return expression;
    }
}