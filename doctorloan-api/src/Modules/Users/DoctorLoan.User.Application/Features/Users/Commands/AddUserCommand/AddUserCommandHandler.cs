using AutoMapper;
using BCrypt.Net;
using DoctorLoan.Application;
using DoctorLoan.Application.Common.Expressions;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Application.Models.Settings;
using DoctorLoan.Domain.Entities.Authorizations;
using DoctorLoan.Domain.Enums.Authorizations;
using DoctorLoan.Domain.Enums.Commons;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DoctorLoan.User.Application.Features.Users;


public class AddUserCommandHandler : ApplicationBaseService<AddUserCommandHandler>, IRequestHandler<AddUserCommand, Result>
{
    private readonly IMapper _mapper;
    private readonly SystemConfiguration _systemConfig;

    public AddUserCommandHandler(ILogger<AddUserCommandHandler> logger, IApplicationDbContext context,
                                    IMapper mapper,
                                    ICurrentRequestInfoService currentRequestInfoService,
                                    IOptions<SystemConfiguration> systemConfig,
                                    ICurrentTranslateService currentTranslateService,
                                    IDateTime dateTime)
    : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
        _mapper = mapper;
        _systemConfig = systemConfig.Value;
    }

    public async Task<Result> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        var roleUser = await _context.Roles.FirstOrDefaultAsync(s => s.Code.ToLower() == "user", cancellationToken);
        if (roleUser == null)
        {
            return Result.Failed(ServiceError.SystemTemporarilyInterrupted(_currentTranslateService));
        }

        #region check email or phone in User table
        if (!string.IsNullOrEmpty(request.Email))
        {
            var existedUserByEmail = await _context.Users.AnyAsync(UserExpressions.GetUserByEmail(request.Email), cancellationToken);
            if (existedUserByEmail)
            {
                return Result.Failed(ServiceError.UserEmailExisted(_currentTranslateService));
            }
        }

        if (!string.IsNullOrEmpty(request.Phone))
        {
            var checkUserByPhone = await _context.Users.FirstOrDefaultAsync(UserExpressions.GetUserByPhone(request.Phone), cancellationToken);
            if (checkUserByPhone != null)
            {
                return ValidationExtention.IsUserBlocked(checkUserByPhone.Status)
                    ? Result.Failed(ServiceError.UserBlocked(_currentTranslateService))
                    : Result.Failed(ServiceError.UserPhoneExisted(_currentTranslateService));
            }
        }
        #endregion

        if (string.IsNullOrEmpty(request.Code))
        {
            var totalUser = await _context.Users.MaxAsync(s => s.Id, cancellationToken);
            request.Code = _systemConfig.DefaultPrefixCode + totalUser.ToString("D7");
        }
        var salt = BCrypt.Net.BCrypt.GenerateSalt(10);

        var info = _mapper.Map<Domain.Entities.Users.User>(request);
        info.UUId = Guid.NewGuid();
        info.FullName = info.FirstName + " " + info.LastName;
        info.Status = request.Status.HasValue ? request.Status.Value : Domain.Enums.Users.UserStatus.Active;
        info.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, salt, false, HashType.SHA512);
        info.UserName = info.Email.Trim().ToLower();
        info.Gender = info.Gender;
        info.DOB = info.DOB;
        info.RoleId = roleUser.Id;
        info.LanguageId = LanguageEnum.VN;
        info.SourcePlatform = SourcePlatform.WebAdmin;

        await _context.Users.AddAsync(info, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        await AddDefaultPermission(info, cancellationToken);

        return Result.Success(info.Id);
    }

    private async Task AddDefaultPermission(Domain.Entities.Users.User user, CancellationToken cancellationToken)
    {
        var listPermissionAction = await _context.PermissionActions.ToListAsync(cancellationToken);
        var listPermission = new List<UserPermission>();
        foreach (var item in listPermissionAction)
        {
            if (item.ActionId == (int)PermissionActionEnum.Delete)
                continue;

            if (item.ModuleId == (int)PermissionModuleEnum.User && item.ActionId != (int)PermissionActionEnum.Read)
                continue;

            listPermission.Add(new UserPermission()
            {
                UserId = user.Id,
                PermissionActionId = item.Id
            });
        }
        if (listPermission.Any())
        {
            await _context.UserPermissions.AddRangeAsync(listPermission, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
