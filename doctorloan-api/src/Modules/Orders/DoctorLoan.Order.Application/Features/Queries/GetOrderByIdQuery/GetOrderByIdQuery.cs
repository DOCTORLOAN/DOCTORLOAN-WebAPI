using AutoMapper;
using DoctorLoan.Application;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Order.Application.Features.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.Order.Application.Features.Queries;

public record GetOrderByIdQuery(int Id) : IRequest<Result<OrderDto>> { }

public class GetOrderByIdQueryHandler : ApplicationBaseService<GetOrderByIdQueryHandler>, IRequestHandler<GetOrderByIdQuery, Result<OrderDto>>
{
    private readonly IMapper _mapper;
    public GetOrderByIdQueryHandler(ILogger<GetOrderByIdQueryHandler> logger,
                                 IApplicationDbContext context,
                                 ICurrentRequestInfoService currentRequestInfoService,
                                 ICurrentTranslateService currentTranslateService,
                                 IDateTime dateTime, IMapper mapper)
        : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
        _mapper = mapper;
    }

    public async Task<Result<OrderDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var info = await _context.Orders
                                .Include(s => s.OrderItems)
                                    .ThenInclude(s => s.ProductItem)
                                        .ThenInclude(s => s.ProductOptions)
                                .Include(s => s.Customer)
                                .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (info == null) return Result.Failed<OrderDto>(ServiceError.NotFound(_currentTranslateService));
        var data = _mapper.Map<OrderDto>(info);

        return Result.Success(data);
    }
}
