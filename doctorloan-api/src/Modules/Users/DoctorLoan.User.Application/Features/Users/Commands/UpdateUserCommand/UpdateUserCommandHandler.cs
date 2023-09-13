using DoctorLoan.Application;
using DoctorLoan.Application.Common.Expressions;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Application.Models.Settings;
using DoctorLoan.Domain.Enums.Commons;
using DoctorLoan.Domain.Enums.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DoctorLoan.User.Application.Features.Users;

public class UpdateUserCommand : IRequest<Result>
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    private string _email;
    public string Email
    {
        get
        {
            if (string.IsNullOrEmpty(_email))
                return string.Empty;
            return _email.Trim();
        }
        set { _email = value; }
    }

    private string _phone;
    public string Phone
    {
        get => _phone.RemoveFirstZero();
        set { _phone = value; }
    }
    public int? Avatar { get; set; }
    public UserStatus? Status { get; set; }
    public DateTime? DOB { get; set; }
    public Gender Gender { get; set; }
}

public class UpdateUserCommandHandler : ApplicationBaseService<UpdateUserCommandHandler>, IRequestHandler<UpdateUserCommand, Result>
{
    public UpdateUserCommandHandler(ILogger<UpdateUserCommandHandler> logger, IApplicationDbContext context,
                                    ICurrentRequestInfoService currentRequestInfoService,
                                    IOptions<SystemConfiguration> systemConfig,
                                    ICurrentTranslateService currentTranslateService,
                                    IDateTime dateTime)
    : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    { }

    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync(new object[] { request.Id }, cancellationToken);
        if (user is null)
            return Result.Failed(ServiceError.UserNotFound(_currentTranslateService));

        #region check email or phone in User table
        if (!string.IsNullOrEmpty(request.Email))
        {
            var existedUserByEmail = await _context.Users.FirstOrDefaultAsync(UserExpressions.GetUserByEmail(request.Email), cancellationToken);
            if (existedUserByEmail != null && existedUserByEmail.Id != user.Id)
                return Result.Failed(ServiceError.UserEmailExisted(_currentTranslateService));
        }

        if (!string.IsNullOrEmpty(request.Phone))
        {
            var checkUserByPhone = await _context.Users.FirstOrDefaultAsync(UserExpressions.GetUserByPhone(request.Phone), cancellationToken);
            if (checkUserByPhone != null && checkUserByPhone.Id != user.Id)
                return Result.Failed(ServiceError.UserPhoneExisted(_currentTranslateService));
        }
        #endregion

        user.UUId = Guid.NewGuid();
        user.FullName = request.FirstName.DisplayFullName(request.LastName);
        if (request.Status.HasValue) user.Status = request.Status.Value;
        if (user.UserName.Trim().ToLower() != request.Email.Trim().ToLower())
            user.UserName = request.Email.Trim().ToLower();

        user.DOB = request.DOB;
        user.FirstName = request.FirstName;
        user.Phone = request.Phone;
        user.Gender = request.Gender;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(user.Id);
    }
}