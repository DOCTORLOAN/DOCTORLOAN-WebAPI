using AutoMapper;
using DoctorLoan.Application;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.Customer.Application.Features.Customers;

public record GetCustomerByIdQuery(int Id) : IRequest<Result<CustomerDto>> { }

public class GetCustomerByIdQueryHandler : ApplicationBaseService<GetCustomerByIdQueryHandler>, IRequestHandler<GetCustomerByIdQuery, Result<CustomerDto>>
{
    private readonly IMapper _mapper;
    public GetCustomerByIdQueryHandler(ILogger<GetCustomerByIdQueryHandler> logger,
                                 IApplicationDbContext context,
                                 ICurrentRequestInfoService currentRequestInfoService,
                                 ICurrentTranslateService currentTranslateService,
                                 IDateTime dateTime, IMapper mapper)
        : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
        _mapper = mapper;
    }

    public async Task<Result<CustomerDto>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var info = await _context.Customers.FindAsync(new object[] { request.Id }, cancellationToken);
        if (info == null) return Result.Failed<CustomerDto>(ServiceError.NotFound(_currentTranslateService));
        var data = _mapper.Map<CustomerDto>(info);

        return Result.Success(data);
    }
}