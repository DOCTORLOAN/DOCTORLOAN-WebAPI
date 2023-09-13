using DoctorLoan.Application;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Domain.Enums.Commons;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.Common.Application.Features.Queries.GetRolesQuery;
public record GetOptionsQuery(OptionType type) : IRequest<Result<List<Option>>>;

public class GetOptionsQueryHandler : ApplicationBaseService<GetOptionsQueryHandler>, IRequestHandler<GetOptionsQuery, Result<List<Option>>>
{

    private readonly ICurrentUserService _currentUserService;

    public GetOptionsQueryHandler(ILogger<GetOptionsQueryHandler> logger,
        ICurrentUserService currentUserService,
        IApplicationDbContext context,
        ICurrentRequestInfoService currentRequestInfoService,
        ICurrentTranslateService currentTranslateService,
        IDateTime dateTime) : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
        _currentUserService = currentUserService;
    }

    public async Task<Result<List<Option>>> Handle(GetOptionsQuery request, CancellationToken cancellationToken)
    {
        var result = new List<Option>();
        if (request.type == OptionType.Role)
        {
            var data = await _context.Roles.Where(s => s.IsActive)
                                                    .OrderBy(s => s.Name)
                                                    .ToListAsync(cancellationToken);
            var roleAdmin = data.FirstOrDefault(s => s.Code == "admin");
            if (roleAdmin is not null)
            {
                if (_currentUserService?.CurrentUser?.RoleId != roleAdmin.Id)
                {
                    data = data.Where(s => s.Id != roleAdmin.Id).ToList();
                }
                result = data.Select(s => new Option(s)).ToList();
            }
        }

        return Result.Success(result);
    }

}

