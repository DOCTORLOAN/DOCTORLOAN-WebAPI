
using AutoMapper;
using DoctorLoan.Application;
using DoctorLoan.Application.Common.Expressions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.User.Application.Features.Users.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.User.Application.Features.Users;

public class GetMeQuery : IRequest<Result<UserDto>>
{
    public class GetMeQueryHandle : ApplicationBaseService<GetMeQueryHandle>, IRequestHandler<GetMeQuery, Result<UserDto>>
    {
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetMeQueryHandle(ILogger<GetMeQueryHandle> logger, IApplicationDbContext context,
            ICurrentRequestInfoService currentRequestInfoService, ICurrentTranslateService currentTranslateService,
            IDateTime dateTime, IMapper mapper,
            ICurrentUserService currentUserService
            ) : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
        {
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<Result<UserDto>> Handle(GetMeQuery request, CancellationToken cancellationToken)
        {
            var userInfo = await _context.Users.FirstOrDefaultAsync(UserExpressions.GetUserById(_currentUserService.UserId), cancellationToken);
            if (userInfo == null) return Result.Failed<UserDto>(ServiceError.UserNotFound(_currentTranslateService));

            var info = _mapper.Map<UserDto>(userInfo);
            return Result.Success(info);
        }
    }
}

