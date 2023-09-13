using DoctorLoan.Domain.Enums.Authorizations;
using Microsoft.AspNetCore.Mvc;

namespace DoctorLoan.Application.Common.Security;
public class AuthorizeAttribute : TypeFilterAttribute
{
    public AuthorizeAttribute(PermissionModuleEnum module, PermissionActionEnum action)
    : base(typeof(AuthorizeActionFilter))
    {
        Arguments = new object[] { module, action };
    }
}
