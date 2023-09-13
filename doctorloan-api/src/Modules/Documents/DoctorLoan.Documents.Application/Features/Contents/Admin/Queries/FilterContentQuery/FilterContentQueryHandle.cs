using DoctorLoan.Application;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Contents.Application.Common.Expressions;
using DoctorLoan.Contents.Application.Features.Contents.Admin.Dtos;
using DoctorLoan.Domain.Entities.Contents;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.Contents.Application.Features.Contents.Admin.Queries;
public class FilterContentQueryHandle : ApplicationBaseService<FilterContentQueryHandle>, IRequestHandler<FilterContentQuery, Result<PaginatedList<ContentInfomationDto>>>
{
    private readonly IEncryptionService _encryptionService;
    public FilterContentQueryHandle(ILogger<FilterContentQueryHandle> logger,
                                 IApplicationDbContext context,
                                 ICurrentRequestInfoService currentRequestInfoService,
                                 ICurrentTranslateService currentTranslateService,
                                 IDateTime dateTime,
                                 IEncryptionService encryptionService)
        : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
        _encryptionService = encryptionService;
    }

    public async Task<Result<PaginatedList<ContentInfomationDto>>> Handle(FilterContentQuery request, CancellationToken cancellationToken)
    {
        var abc = _encryptionService.Encrypt("abc");
        var condition = PredicateBuilder.True<Content>();

        #region Add condition
        if (!string.IsNullOrWhiteSpace(request.KeyWord))
        {
            condition = condition.And(ContentExpression.IsContains(request.KeyWord));
        }
        if (!string.IsNullOrWhiteSpace(request.Code))
        {
            condition = condition.And(s => s.Code.ToLower().Contains(request.Code.ToLower()));
        }
        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            condition = condition.And(s => s.Name.ToLower().Contains(request.Name.ToLower()));
        }
        if (request.Status.HasValue)
        {
            condition = condition.And(s => s.Status == request.Status);
        }
        if (request.Type.HasValue)
        {
            condition = condition.And(s => s.Type == request.Type);
        }
        #endregion

        var queryable = _context.Contents.Where(condition);
        queryable = request.Params.sort.Trim().ToLower() switch
        {
            "name" => queryable.Sort(m => m.Name, request.SortAsc),
            "code" => queryable.Sort(m => m.Code, request.SortAsc),
            "status" => queryable.Sort(m => m.Status, request.SortAsc),
            "created" => queryable.Sort(m => m.Created, request.SortAsc),
            _ => queryable.Sort(m => m.Id, asc: false),
        };
        var (page, take, sort, asc) = request.Params;
        var result = await queryable.Select(m => new ContentInfomationDto(m)).ToPagedListAsync(page, take, cancellationToken);

        return Result.Success(result);
    }
}
