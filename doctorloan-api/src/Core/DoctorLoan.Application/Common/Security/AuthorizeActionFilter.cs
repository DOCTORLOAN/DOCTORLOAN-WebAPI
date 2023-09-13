using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Domain.Enums.Authorizations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DoctorLoan.Application.Common.Security;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeActionFilter : Attribute, IAuthorizationFilter
{
    private readonly PermissionModuleEnum _module;
    private readonly PermissionActionEnum _action;
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public AuthorizeActionFilter(PermissionModuleEnum module, PermissionActionEnum action
        , IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _module = module;
        _action = action;
        _context = context;
        _currentUserService = currentUserService;
        _currentUserService = currentUserService;
    }
    public void OnAuthorization(AuthorizationFilterContext authorizationFilterContext)
    {
        var isAuthenticated = authorizationFilterContext.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        if (!isAuthenticated || _currentUserService.CurrentUser is null)
        {
            authorizationFilterContext.Result = new UnauthorizedResult();
            return;
        }

        var allowAnonymous = authorizationFilterContext.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous)
            return;

        var roleAdmin = _context.Roles.FirstOrDefault(s => s.Code == "admin");

        if (_currentUserService.CurrentUser.RoleId != roleAdmin.Id)
        {
            if ((int)_module > 0 && (int)_action > 0)
            {
                var isAuthorized = _context.UserPermissions.Any(s => s.UserId == _currentUserService.UserId
                                                && s.PermissionAction.ModuleId == (int)_module && s.PermissionAction.ActionId == (int)_action
                                                && s.PermissionAction.IsDelete == false);
                if (!isAuthorized)
                {
                    authorizationFilterContext.Result = new ForbidResult();
                    return;
                }
            }
        }
    }
}