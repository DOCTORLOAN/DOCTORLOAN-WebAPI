using DoctorLoan.Application;
using DoctorLoan.Application.Interfaces.Authenticates;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Authenticates;
using DoctorLoan.Application.Models.Commons;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.Authen.Application.Features.Authenticates;

public class RefreshTokenCommand : IRequest<Result<JWTTokens>>
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
};

public class RefreshTokenCommandHandler : ApplicationBaseService<RefreshTokenCommandHandler>, IRequestHandler<RefreshTokenCommand, Result<JWTTokens>>
{
    private readonly IJWTServices _jwtService;

    public RefreshTokenCommandHandler(ILogger<RefreshTokenCommandHandler> logger,
                             IApplicationDbContext context,
                             ICurrentRequestInfoService currentRequestInfoService,
                             ICurrentTranslateService currentTranslateService,
                             IDateTime dateTime,
                             IJWTServices jwtService)
    : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
        _jwtService = jwtService;
    }

    public async Task<Result<JWTTokens>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var principal = await _jwtService.GetPrincipalFromExpiredToken(request.AccessToken);
        if (principal == null) return Result.Failed<JWTTokens>("Invalid Token");
        var username = principal.Identity?.Name;
        //Do some here: retrieve the saved refresh token from database
        //var newJwtToken = await _jwtService.GenerateToken(new UserSignIn() { UserName = username });
        //if (newJwtToken == null)
        //{
        //    return Result.Failed<JWTTokens>("Invalid attempt!");
        //}
        //Do some here: Update status for new fresh token into database
        return Result.Success(new JWTTokens());
    }
}
