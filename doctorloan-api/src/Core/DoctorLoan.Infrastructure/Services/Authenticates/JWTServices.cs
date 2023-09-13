using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DoctorLoan.Application.Interfaces.Authenticates;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Authenticates;
using DoctorLoan.Application.Models.Settings;
using DoctorLoan.Domain.Entities.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DoctorLoan.Infrastructure.Services.Authenticates;
public class JWTServices : IJWTServices
{
    #region Fields
    private readonly JWTTokenConfiguration _jwtTokenConfiguration;
    private readonly IApplicationDbContext _context;
    #endregion

    #region Ctor
    public JWTServices(IApplicationDbContext context, IOptions<JWTTokenConfiguration> jwtTokenConfiguration,
        IOptions<SystemConfiguration> systemConfiguration)
    {
        _context = context;
        _jwtTokenConfiguration = jwtTokenConfiguration.Value;
    }
    #endregion

    public Task<JWTTokens> GenerateToken(User user)
    {
        var key = _jwtTokenConfiguration.Key;

        var minuteExpired = _jwtTokenConfiguration.Expires;
        var expiredTime = DateTime.UtcNow.AddMinutes(minuteExpired);
        var tokenhandler = new JwtSecurityTokenHandler();
        var tkey = Encoding.UTF8.GetBytes(key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.RoleId.ToString()),
                new Claim(ClaimTypes.System, "this is link URL get current user's permission")
            }),
            Expires = expiredTime,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tkey), SecurityAlgorithms.HmacSha256Signature)
        };
        var accessToken = tokenhandler.CreateToken(tokenDescriptor);
        var refreshToken = GenerateRefreshToken();

        return Task.FromResult(new JWTTokens
        {
            AccessToken = tokenhandler.WriteToken(accessToken),
            RefeshToken = refreshToken,
            ExpiresOn = ((DateTimeOffset)expiredTime).ToUnixTimeSeconds(),
            ExpiresIn = minuteExpired * 60
        });
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    public Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token)
    {
        if (string.IsNullOrEmpty(token)) return null;
        var key = _jwtTokenConfiguration.Key;
        var tkey = Encoding.UTF8.GetBytes(key);

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(tkey),
            ClockSkew = TimeSpan.Zero
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            return null;
        }
        return Task.FromResult(principal);
    }
}
