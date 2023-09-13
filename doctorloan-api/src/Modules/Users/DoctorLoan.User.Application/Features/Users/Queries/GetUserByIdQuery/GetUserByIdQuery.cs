using AutoMapper;
using DoctorLoan.Application;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Domain.Enums.Users;
using DoctorLoan.User.Application.Features.Users.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.User.Application.Features.Users;

public record GetUserByIdQuery(int Id) : IRequest<Result<UserDto>>;

public class GetUserByIdQueryHandle : ApplicationBaseService<DeleteUserCommandHandle>, IRequestHandler<GetUserByIdQuery, Result<UserDto>>
{

    private readonly IMapper _mapper;

    public GetUserByIdQueryHandle(ILogger<DeleteUserCommandHandle> logger,
                            IMapper mapper,
                            IApplicationDbContext context,
                            ICurrentRequestInfoService currentRequestInfoService,
                            ICurrentTranslateService currentTranslateService,
                            IDateTime dateTime)
                            : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
        _mapper = mapper;
    }

    public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
                                .Include(x => x.UserDetail)
                                .Include(s => s.Role)
                                .Include(s => s.UserActivities.Where(s => s.ActivityType == UserActivityType.SignIn))
                                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (user == null)
            return Result.Failed<UserDto>(ServiceError.NotFound(_currentTranslateService));

        var result = _mapper.Map<UserDto>(user);

        return Result.Success(result);
    }
}
