using AutoMapper;
using AutoMapper.QueryableExtensions;
using DoctorLoan.Application;
using DoctorLoan.Application.Common.Expressions;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Domain.Enums.Users;
using DoctorLoan.User.Application.Features.Users.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.User.Application.Features.Users;

public class FilterUserQueryHandler : ApplicationBaseService<FilterUserQueryHandler>, IRequestHandler<FilterUserQuery, Result<PaginatedList<UserDto>>>
{
    private readonly IMapper _mapper;
    public FilterUserQueryHandler(ILogger<FilterUserQueryHandler> logger,
                                 IApplicationDbContext context,
                                 ICurrentRequestInfoService currentRequestInfoService,
                                 ICurrentTranslateService currentTranslateService,
                                 IDateTime dateTime, IMapper mapper)
        : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
        _mapper = mapper;
    }

    public async Task<Result<PaginatedList<UserDto>>> Handle(FilterUserQuery request, CancellationToken cancellationToken)
    {
        var condition = PredicateBuilder.Create<Domain.Entities.Users.User>(s => s.Role.Code != "admin");
        #region add conditions
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            condition = condition.And(UserExpressions.IsContains(request.Search));
        }

        if (request.Filter.Status.HasValue)
        {
            condition = condition.And(s => s.Status == request.Filter.Status);
        }

        if (request.Filter.Role.HasValue && request.Filter.Role > 0)
        {
            condition = condition.And(s => s.RoleId == request.Filter.Role);
        }

        if (request.Filter.LastLogin.HasValue)
        {
            var dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(request.Filter.LastLogin.Value);
            condition = condition.And(s => s.UserActivities.Any(u => u.Created >= dateTimeOffset && u.ActivityType == UserActivityType.SignIn));
        }
        condition = condition.And(s => !UserExpressions.ListStatusDeleted.Contains(s.Status));
        #endregion

        var queryable = _context.Users.Include(s => s.Role)
                                    .Where(condition);
        queryable = request.Params.sort.Trim().ToLower() switch
        {
            "name" => queryable.Sort(m => m.FullName, request.SortAsc),
            "code" => queryable.Sort(m => m.Code, request.SortAsc),
            "status" => queryable.Sort(m => m.Status, request.SortAsc),
            _ => queryable.Sort(m => m.Id, asc: false),
        };
        var (page, take, sort, asc) = request.Params;
        var result = await queryable.ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                                    .ToPagedListAsync(page, take, cancellationToken);
        return Result.Success(result);
    }
}
