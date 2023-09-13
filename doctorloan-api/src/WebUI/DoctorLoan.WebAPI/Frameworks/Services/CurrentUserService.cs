using System.Security.Claims;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Models.Users;
using DoctorLoan.Domain.Enums.Commons;

namespace DoctorLoan.WebAPI.Frameworks.Services;

public class CurrentUserService : ICurrentUserService
{
    private int _currentLanguageId;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int UserId => int.Parse(_httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

    public LanguageEnum CurrentLanguage
    {
        get
        {
            if (_currentLanguageId == 0)
            {
                _currentLanguageId = _httpContextAccessor?.HttpContext?.Request.Headers["Accept-Language"].ToString() == "en-US" ? (int)LanguageEnum.EN : (int)LanguageEnum.VN;
            }

            return (LanguageEnum)_currentLanguageId;
        }
    }

    public CurrentUserInfo? CurrentUser
    {
        get
        {
            return GetCurrentUser();
        }
    }

    #region private
    private CurrentUserInfo? GetCurrentUser()
    {
        var currentUser = _httpContextAccessor?.HttpContext?.User;
        if (currentUser is null) return null;

        var isAuthenticated = currentUser.Identity?.IsAuthenticated ?? false;
        if (!isAuthenticated)
            return null;

        _ = int.TryParse(currentUser.FindFirstValue(ClaimTypes.NameIdentifier), out int userId);
        _ = int.TryParse(currentUser.FindFirstValue(ClaimTypes.Role), out int roleId);
        var fullName = currentUser.FindFirstValue(ClaimTypes.Name);

        if (userId <= 0)
        {
            return null;
        }

        return new CurrentUserInfo()
        {
            Id = userId,
            FullName = fullName,
            RoleId = roleId
        };
    }
    #endregion
}
