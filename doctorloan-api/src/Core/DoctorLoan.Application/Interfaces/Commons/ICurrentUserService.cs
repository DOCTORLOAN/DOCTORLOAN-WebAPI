using DoctorLoan.Application.Models.Users;
using DoctorLoan.Domain.Enums.Commons;

namespace DoctorLoan.Application.Interfaces.Commons;

public interface ICurrentUserService
{
    int UserId { get; }
    CurrentUserInfo? CurrentUser { get; }
    LanguageEnum CurrentLanguage { get; }
}
