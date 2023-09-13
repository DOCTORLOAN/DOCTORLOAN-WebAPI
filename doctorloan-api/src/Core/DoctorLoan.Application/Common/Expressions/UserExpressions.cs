using System.Linq.Expressions;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Domain.Entities.Users;
using DoctorLoan.Domain.Enums.Users;
using Microsoft.EntityFrameworkCore;

namespace DoctorLoan.Application.Common.Expressions;

public static class UserExpressions
{
    public static readonly List<UserStatus> ListStatusInActive = new List<UserStatus>() { UserStatus.Block, UserStatus.Removed, UserStatus.Deleted };
    public static readonly List<UserStatus> ListStatusDeleted = new List<UserStatus>() { UserStatus.Deleted };

    public static Expression<Func<User, bool>> GetAllBranchChild(LTree parentTree) => x =>
            !string.IsNullOrEmpty(x.ParentTreeId) && x.ParentTreeId.Value.IsDescendantOf(parentTree)
            && !ListStatusInActive.Contains(x.Status);

    public static Expression<Func<User, bool>> GetBranchByLevel(LTree parentTree, int level) => x =>
        !string.IsNullOrEmpty(x.ParentTreeId) && x.ParentTreeId.Value.MatchesLQuery($"{parentTree}.*{{{level}}}")
        && !ListStatusInActive.Contains(x.Status);

    public static Expression<Func<User, bool>> FindParentBranch(LTree parentTree, int level) => x =>
            x.ParentTreeId.HasValue && x.ParentTreeId.Value.IsDescendantOf(parentTree)
            && x.ParentTreeId.Value.NLevel == level
            && !ListStatusInActive.Contains(x.Status);

    public static Expression<Func<User, bool>> IsActiveUser(bool onlyActiveUser = true) => x =>
            !onlyActiveUser || !ListStatusInActive.Contains(x.Status);

    public static Expression<Func<User, bool>> IsDuplicatedCode(string code) => x =>
            x.Code == code && !ListStatusInActive.Contains(x.Status);

    public static Expression<Func<User, bool>> IsBlockCode(string code) => x =>
            x.Code == code && ListStatusInActive.Contains(x.Status);

    public static Expression<Func<User, bool>> GetUserByEmail(string email) => x =>
            x.Email == email && !ListStatusInActive.Contains(x.Status);

    public static Expression<Func<User, bool>> GetUserByPhone(string phone) => x =>
            x.Phone == "84" + phone;

    public static Expression<Func<User, bool>> GetUserById(int userId) => x =>
           x.Id == userId && !ListStatusInActive.Contains(x.Status);

    public static Expression<Func<User, bool>> IsContains(string keyword)
    {
        var search = keyword.BuildFullTextSearchTerm();

        Expression<Func<User, bool>> expression = x => EF.Functions.Like(x.FullName.ToLower(), search)
                                                || EF.Functions.Like(x.Email.ToLower(), search)
                                                || EF.Functions.Like(("0" + x.Phone), search);

        return expression;
    }

    public static Expression<Func<User, bool>> IsLike(string keyword)
    {
        var searchTerm = keyword.Trim().Split(" ").Where(x => !string.IsNullOrEmpty(x)).ToList();
        var searchText = keyword.BuildSearchTerm();

        Expression<Func<User, bool>> expression = x => EF.Functions.Like(x.FullName.ToLower(), searchText)
                                                || EF.Functions.Like(x.Code.ToLower(), searchText)
                                                || EF.Functions.Like(("0" + x.Phone), searchText);
        return expression;
    }

    public static Expression<Func<User, bool>> IsLikeFull(string keyword)
    {
        var search = keyword.BuildFullTextSearchTerm();

        Expression<Func<User, bool>> expression = x => EF.Functions.Like(x.FullName.ToLower(), search)
                                                || EF.Functions.Like(x.Code.ToLower(), search)
                                                || EF.Functions.Like(("0" + x.Phone), search);
        return expression;
    }
}
