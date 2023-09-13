using System.Linq.Expressions;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Domain.Enums.Users;
using Microsoft.EntityFrameworkCore;

namespace DoctorLoan.User.Application.Commons.Expressions;
public static class UserExpressions
{
    public static readonly List<UserStatus> ListStatusInActive = new List<UserStatus>() { UserStatus.Block, UserStatus.Removed, UserStatus.Deleted };

    public static Expression<Func<Domain.Entities.Users.User, bool>> GetAllBranchChild(LTree parentTree) => x =>
            !string.IsNullOrEmpty(x.ParentTreeId) && x.ParentTreeId.Value.IsDescendantOf(parentTree)
            && !ListStatusInActive.Contains(x.Status);

    public static Expression<Func<Domain.Entities.Users.User, bool>> GetBranchByLevel(LTree parentTree, int level) => x =>
        !string.IsNullOrEmpty(x.ParentTreeId) && x.ParentTreeId.Value.MatchesLQuery($"{parentTree}.*{{{level}}}")
        && !ListStatusInActive.Contains(x.Status);

    public static Expression<Func<Domain.Entities.Users.User, bool>> FindParentBranch(LTree parentTree, int level) => x =>
            x.ParentTreeId.HasValue && x.ParentTreeId.Value.IsDescendantOf(parentTree)
            && x.ParentTreeId.Value.NLevel == level
            && !ListStatusInActive.Contains(x.Status);

    public static Expression<Func<Domain.Entities.Users.User, bool>> IsActiveUser(bool onlyActiveUser = true) => x =>
            !onlyActiveUser || !ListStatusInActive.Contains(x.Status);

    public static Expression<Func<Domain.Entities.Users.User, bool>> IsDuplicatedCode(string code) => x =>
            x.Code == code && !ListStatusInActive.Contains(x.Status);

    public static Expression<Func<Domain.Entities.Users.User, bool>> IsBlockCode(string code) => x =>
            x.Code == code && ListStatusInActive.Contains(x.Status);

    public static Expression<Func<Domain.Entities.Users.User, bool>> GetUserByEmail(string email) => x =>
            x.Email == email && !ListStatusInActive.Contains(x.Status);

    public static Expression<Func<Domain.Entities.Users.User, bool>> GetUserByPhone(string phone) => x =>
            x.Phone == "84" + phone;

    public static Expression<Func<Domain.Entities.Users.User, bool>> GetUserById(int userId) => x =>
           x.Id == userId && !ListStatusInActive.Contains(x.Status);

    public static Expression<Func<Domain.Entities.Users.User, bool>> IsContains(string keyword)
    {
        var search = keyword.BuildFullTextSearchTerm();

        Expression<Func<Domain.Entities.Users.User, bool>> expression = x => EF.Functions.Like(x.FullName.ToLower(), search)
                                                || EF.Functions.Like(x.Email.ToLower(), search)
                                                || EF.Functions.Like("84" + x.Phone, search)
                                                || EF.Functions.Like(x.Phone, search);

        return expression;
    }

    public static Expression<Func<Domain.Entities.Users.User, bool>> IsLike(string keyword)
    {
        var searchTerm = keyword.Trim().Split(" ").Where(x => !string.IsNullOrEmpty(x)).ToList();
        var searchText = keyword.BuildSearchTerm();

        Expression<Func<Domain.Entities.Users.User, bool>> expression = x => EF.Functions.Like(x.FullName.ToLower(), searchText)
                                                || EF.Functions.Like(x.Code.ToLower(), searchText)
                                                || EF.Functions.Like("84" + x.Phone, searchText)
                                                || EF.Functions.Like(x.Email, searchText);
        return expression;
    }
}

