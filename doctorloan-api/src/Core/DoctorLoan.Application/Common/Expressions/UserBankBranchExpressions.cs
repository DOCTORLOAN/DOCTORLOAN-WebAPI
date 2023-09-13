using System.Linq.Expressions;
using DoctorLoan.Domain.Entities.Users;
using DoctorLoan.Domain.Enums.Users;

namespace DoctorLoan.Application.Common.Expressions;

public static class UserBankBranchExpressions
{
    public static Expression<Func<UserBankBranch, bool>> GetUserAccountNosDedicated(List<string> userAccountNos) => x =>
           userAccountNos.Contains(x.AccountNo) && x.IsDedicated == true && x.Status == UserBankBranchStatus.Verified;
}
