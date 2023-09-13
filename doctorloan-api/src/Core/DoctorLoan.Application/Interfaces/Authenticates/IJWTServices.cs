using System.Security.Claims;
using DoctorLoan.Application.Models.Authenticates;
using DoctorLoan.Domain.Entities.Users;

namespace DoctorLoan.Application.Interfaces.Authenticates;
public interface IJWTServices
{
    Task<JWTTokens> GenerateToken(User user);
    Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token);
}
