using AutoMapper;
using DoctorLoan.Application;
using DoctorLoan.Application.Common.Expressions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Domain.Entities.Addresses;
using DoctorLoan.Domain.Entities.Customers;
using DoctorLoan.Domain.Enums.Addresses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DoctorLoan.Customer.Application.Features.CustomerAddresses;

public class UpdateCustomerAddressCommand : IRequest<Result<bool>>
{
    public int Id { get; set; }

    [JsonIgnore]
    public int CustomerId { get; set; }

    public AddressType Type { get; set; }
    public string FullName { get; set; }
    public string Phone { get; set; }
    public string Remarks { get; set; }
    public bool IsDefault { get; set; }

    public string AddressLine { get; set; }
    public int ProvinceId { get; set; }
    public int DistrictId { get; set; }
    public int WardId { get; set; }
}

public class UpdateCustomerAddressCommandHandler : ApplicationBaseService<UpdateCustomerAddressCommandHandler>, IRequestHandler<UpdateCustomerAddressCommand, Result<bool>>
{
    private readonly IMapper _mapper;

    public UpdateCustomerAddressCommandHandler(ILogger<UpdateCustomerAddressCommandHandler> logger, IApplicationDbContext context,
                                    ICurrentRequestInfoService currentRequestInfoService,
                                    IMapper mapper,
                                    ICurrentTranslateService currentTranslateService,
                                    IDateTime dateTime)
    : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
        _mapper = mapper;
    }

    public async Task<Result<bool>> Handle(UpdateCustomerAddressCommand request, CancellationToken cancellationToken)
    {
        var customerAddress = await _context.CustomerAddresses.FindAsync(new object[] { request.Id }, cancellationToken);
        if (customerAddress is null)
        {
            return Result.Failed<bool>(ServiceError.NotFound(_currentTranslateService));
        }

        if (customerAddress.CustomerId != request.CustomerId)
        {
            return Result.Failed<bool>(ServiceError.Invalid(_currentTranslateService));
        }

        customerAddress.FullName = request.FullName;
        customerAddress.Phone = request.Phone;
        customerAddress.Remarks = request.Remarks;
        customerAddress.IsDefault = request.IsDefault;

        if (customerAddress.IsDefault)
        {
            var listCustAddress = _context.CustomerAddresses.Where(s => s.CustomerId == request.CustomerId);
            if (listCustAddress.Any())
            {
                await listCustAddress.ForEachAsync(s => s.IsDefault = false, cancellationToken);
            }
        }

        var address = await _context.Addresses.FirstOrDefaultAsync(AddressExpression.IsDuplicated(request.AddressLine, request.ProvinceId, request.DistrictId, request.WardId), cancellationToken);
        if (address == null)
        {
            var newAddressEntity = _mapper.Map<Address>(request);

            newAddressEntity.CountryId = (int)AddressSystemIds.VN;
            newAddressEntity.CustomerAddresses.Add(new CustomerAddress()
            {
                FullName = request.FullName,
                Phone = request.Phone,
                CustomerId = request.CustomerId
            });
            await _context.Addresses.AddAsync(newAddressEntity, cancellationToken);
        }
        else
        {
            if (address.Id != customerAddress.AddressId)
            {
                address.CustomerAddresses.Add(new CustomerAddress()
                {
                    FullName = request.FullName,
                    Phone = request.Phone,
                    CustomerId = request.CustomerId
                });
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success(true);
    }
}

