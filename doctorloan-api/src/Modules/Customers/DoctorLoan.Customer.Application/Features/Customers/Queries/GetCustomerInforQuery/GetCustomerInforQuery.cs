using AutoMapper;
using DoctorLoan.Application;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.Customer.Application.Features.Customers;

public class GetCustomerInforQuery : IRequest<Result<CustomerDto>>
{
    public class GetCustomerInforQueryHandler : ApplicationBaseService<GetCustomerInforQueryHandler>, IRequestHandler<GetCustomerInforQuery, Result<CustomerDto>>
    {
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetCustomerInforQueryHandler(ILogger<GetCustomerInforQueryHandler> logger, IApplicationDbContext context,
            ICurrentRequestInfoService currentRequestInfoService, ICurrentTranslateService currentTranslateService,
            IDateTime dateTime, IMapper mapper,
            ICurrentUserService currentUserService
            ) : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
        {
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<Result<CustomerDto>> Handle(GetCustomerInforQuery request, CancellationToken cancellationToken)
        {
            var customerInfo = await _context.Customers.FindAsync(new object[] { _currentUserService.UserId }, cancellationToken);
            if (customerInfo == null) return Result.Failed<CustomerDto>(ServiceError.NotFound(_currentTranslateService));

            var info = _mapper.Map<CustomerDto>(customerInfo);
            return Result.Success(info);
        }
    }
}

