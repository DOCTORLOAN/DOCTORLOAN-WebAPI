using System.Text.Json.Serialization;
using AutoMapper;
using DoctorLoan.Application;
using DoctorLoan.Application.Common.Expressions;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Domain.Entities.Addresses;
using DoctorLoan.Domain.Entities.Customers;
using DoctorLoan.Domain.Enums.Addresses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.Customer.Application.Features.CustomerAddresses;

public class AddCustomerAddressCommand : IRequest<Result<bool>>
{
    [JsonIgnore]
    public int CustomerId { get; set; }
    public AddressType Type { get; set; }
    public string FullName { get; set; }
    private string _phone;
    public string Phone
    {
        get => _phone.RemoveFirstZero();
        set { _phone = value; }
    }
    public string Remarks { get; set; }
    public bool IsDefault { get; set; }

    public string AddressLine { get; set; }
    public int? ProvinceId { get; set; }
    public int? DistrictId { get; set; }
    public int? WardId { get; set; }
}


public class AddCustomerAddressCommandHandler : ApplicationBaseService<AddCustomerAddressCommandHandler>, IRequestHandler<AddCustomerAddressCommand, Result<bool>>
{
    private readonly IMapper _mapper;

    public AddCustomerAddressCommandHandler(ILogger<AddCustomerAddressCommandHandler> logger, IApplicationDbContext context,
                                    ICurrentRequestInfoService currentRequestInfoService,
                                    IMapper mapper,
                                    ICurrentTranslateService currentTranslateService,
                                    IDateTime dateTime)
    : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
        _mapper = mapper;
    }

    public async Task<Result<bool>> Handle(AddCustomerAddressCommand request, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers.FindAsync(new object[] { request.CustomerId }, cancellationToken);
        if (customer is null)
        {
            return Result.Failed<bool>(ServiceError.NotFound(_currentTranslateService));
        }

        if (request.IsDefault)
        {
            var listCustAddress = await _context.CustomerAddresses.Where(s => s.CustomerId == customer.Id).ToListAsync(cancellationToken);
            if (listCustAddress.Any())
            {
                listCustAddress.ForEach(s => s.IsDefault = false);
            }
        }

        var address = await _context.Addresses.FirstOrDefaultAsync(AddressExpression.IsDuplicated(request.AddressLine, request.ProvinceId, request.DistrictId, request.WardId), cancellationToken);
        if (address == null)
        {
            var newAddressEntity = _mapper.Map<Address>(request);

            newAddressEntity.CountryId = (int)AddressSystemIds.VN;
            newAddressEntity.CustomerAddresses.Add(new CustomerAddress()
            {
                Type = request.Type,
                FullName = request.FullName,
                Phone = request.Phone,
                CustomerId = customer.Id,
                IsDefault = request.IsDefault
            });
            await _context.Addresses.AddAsync(newAddressEntity, cancellationToken);
        }
        else
        {
            var customerAddress = await _context.CustomerAddresses.AnyAsync(s => s.CustomerId == customer.Id && s.AddressId == address.Id, cancellationToken);
            if (!customerAddress)
            {
                address.CustomerAddresses.Add(new CustomerAddress()
                {
                    Type = request.Type,
                    FullName = request.FullName,
                    Phone = request.Phone,
                    CustomerId = customer.Id,
                    IsDefault = request.IsDefault
                });
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success(true);
    }
}

