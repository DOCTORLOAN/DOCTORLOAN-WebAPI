using BCrypt.Net;
using DoctorLoan.Application;
using DoctorLoan.Application.Interfaces.Authenticates;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Authenticates;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Application.Models.Users;
using DoctorLoan.Domain.Entities.Users;
using DoctorLoan.Domain.Enums.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.Authen.Application.Features.Authenticates;

public class SignInCommand : UserSignIn, IRequest<Result<JWTTokens>> { };

public class SignInCommandHandler : ApplicationBaseService<SignInCommandHandler>, IRequestHandler<SignInCommand, Result<JWTTokens>>
{
    private readonly IJWTServices _jwtService;

    public SignInCommandHandler(ILogger<SignInCommandHandler> logger,
                             IApplicationDbContext context,
                             ICurrentRequestInfoService currentRequestInfoService,
                             ICurrentTranslateService currentTranslateService,
                             IDateTime dateTime,
                             IJWTServices jwtService)
    : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
        _jwtService = jwtService;
    }

    public async Task<Result<JWTTokens>> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(s => s.UserName.ToLower() == request.UserName.ToLower(), cancellationToken);
        if (user == null) return Result.Failed<JWTTokens>(ServiceError.UserNameOrPasswordInvalid(_currentTranslateService));
        bool verified = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash, false, HashType.SHA512);
        if (!verified) return Result.Failed<JWTTokens>(ServiceError.UserNameOrPasswordInvalid(_currentTranslateService));

        // userId, roleId, Username, firstname, lastname

        var token = await _jwtService.GenerateToken(user);
        if (token == null)
            return Result.Failed<JWTTokens>(ServiceError.DefaultError);

        var log = new UserActivity()
        {
            UserId = user.Id,
            ActivityType = UserActivityType.SignIn
        };
        await _context.UserActivities.AddAsync(log, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success(token);
    }
}
