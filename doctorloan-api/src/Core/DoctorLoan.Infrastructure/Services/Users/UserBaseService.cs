using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Interfaces.Users;

namespace DoctorLoan.Infrastructure.Services;
public class UserBaseService : IUserBaseService
{
    #region Fields
    private readonly IApplicationDbContext _context;
    #endregion

    #region Ctor
    public UserBaseService(IApplicationDbContext context)
    {
        _context = context;
    }
    #endregion

    #region Methods
    #endregion
}
