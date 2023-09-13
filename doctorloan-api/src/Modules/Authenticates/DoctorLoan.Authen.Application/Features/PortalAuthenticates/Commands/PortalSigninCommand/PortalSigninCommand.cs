using System.Security.Claims;
using BCrypt.Net;
using DoctorLoan.Application;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.Authen.Application.Features.PortalAuthenticates;

public class PortalSigninCommand : IRequest<Result<bool>>
{
    public string UserName { get; set; }
    public string Password { get; set; }
};

public class PortalSigninCommandHandler : ApplicationBaseService<PortalSigninCommandHandler>, IRequestHandler<PortalSigninCommand, Result<bool>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public PortalSigninCommandHandler(ILogger<PortalSigninCommandHandler> logger,
                             IApplicationDbContext context,
                             ICurrentRequestInfoService currentRequestInfoService,
                             ICurrentTranslateService currentTranslateService,
                             IDateTime dateTime,
                             IHttpContextAccessor httpContextAccessor)
    : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<bool>> Handle(PortalSigninCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Customers.FirstOrDefaultAsync(s => s.Phone.ToLower() == request.UserName.ToLower() || s.Email.ToLower() == request.UserName.ToLower(), cancellationToken);
        if (user == null) return Result.Failed<bool>(ServiceError.UserNameOrPasswordInvalid(_currentTranslateService));

        bool verified = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash, false, HashType.SHA512);
        if (!verified) return Result.Failed<bool>(ServiceError.UserNameOrPasswordInvalid(_currentTranslateService));

        //A claim is a statement about a subject by an issuer and    
        //represent attributes of the subject that are useful in the context of authentication and authorization operations.
        var claims = new List<Claim>() {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.FullName),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.MobilePhone, user.Phone),
                    };

        //Initialize a new instance of the ClaimsIdentity with the claims and authentication scheme    
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        //Initialize a new instance of the ClaimsPrincipal with ClaimsIdentity    
        var principal = new ClaimsPrincipal(identity);

        //SignInAsync is a Extension method for Sign in a principal for the specified scheme.    
        await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties());

        return Result.Success<bool>(true);
    }
}