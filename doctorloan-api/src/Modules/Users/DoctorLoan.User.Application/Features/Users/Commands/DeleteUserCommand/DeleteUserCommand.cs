using DoctorLoan.Application;
using DoctorLoan.Application.Common.Expressions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Domain.Enums.Users;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.User.Application.Features.Users;
public record DeleteUserCommand(int UserId) : IRequest<Result<bool>>;

public class DeleteUserCommandHandle : ApplicationBaseService<DeleteUserCommandHandle>, IRequestHandler<DeleteUserCommand, Result<bool>>
{

    public DeleteUserCommandHandle(ILogger<DeleteUserCommandHandle> logger,
    IApplicationDbContext context,
    ICurrentRequestInfoService currentRequestInfoService,
    ICurrentTranslateService currentTranslateService,
    IDateTime dateTime,
    ICurrentUserService currentUserService)
    : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    { }

    public async Task<Result<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Users.FindAsync(new object[] { request.UserId }, cancellationToken);
        if (entity == null || entity.Status == UserStatus.Deleted) return Result.Failed<bool>(ServiceError.UserNotFound(_currentTranslateService));
        if (UserExpressions.ListStatusInActive.Contains(entity.Status))
        {
            return Result.Failed<bool>(ServiceError.UserBlocked(_currentTranslateService));
        }

        entity.Status = UserStatus.Removed;
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(true);
    }
}
