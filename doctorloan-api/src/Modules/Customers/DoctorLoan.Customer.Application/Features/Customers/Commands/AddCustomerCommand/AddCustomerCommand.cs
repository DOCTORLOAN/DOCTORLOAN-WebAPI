using AutoMapper;
using BCrypt.Net;
using DoctorLoan.Application;
using DoctorLoan.Application.Common.Expressions;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Application.Models.Settings;
using DoctorLoan.Customer.Application.Commons.Expressions;
using DoctorLoan.Domain.Entities.Addresses;
using DoctorLoan.Domain.Entities.Customers;
using DoctorLoan.Domain.Enums.Addresses;
using DoctorLoan.Domain.Enums.Commons;
using DoctorLoan.Domain.Enums.Customers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DoctorLoan.Customer.Application.Features.Customers;

public class AddCustomerCommand : IRequest<Result<int>>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    private string _phone;
    public string Phone
    {
        get => _phone.RemoveFirstZero();
        set { _phone = value; }
    }

    public string Email { get; set; }
    public DateTimeOffset? DOB { get; set; }
    public Gender Gender { get; set; }
    public string Password { get; set; }

    public string AddressLine { get; set; }
    public int? ProvinceId { get; set; }
    public int? DistrictId { get; set; }
    public int? WardId { get; set; }

    public CustomerSource Source { get; set; }
}


public class AddCustomerCommandHandler : ApplicationBaseService<AddCustomerCommandHandler>, IRequestHandler<AddCustomerCommand, Result<int>>
{
    private readonly IMapper _mapper;
    private readonly SystemConfiguration _systemConfig;

    public AddCustomerCommandHandler(ILogger<AddCustomerCommandHandler> logger, IApplicationDbContext context,
                                    ICurrentRequestInfoService currentRequestInfoService,
                                    IMapper mapper,
                                    ICurrentTranslateService currentTranslateService,
                                    IDateTime dateTime,
                                    IOptions<SystemConfiguration> systemConfig)
    : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
        _mapper = mapper;
        _systemConfig = systemConfig.Value;
    }

    public async Task<Result<int>> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
    {
        var customerExisted = await _context.Customers.FirstOrDefaultAsync(CustomerExpression.GetCustomerByPhone(request.Phone), cancellationToken);
        if (customerExisted != null)
        {
            return Result.Failed<int>("Số điện thoại đã được sử dụng tài khoản khác.");
        }

        var customerExistedEmail = await _context.Customers.FirstOrDefaultAsync(CustomerExpression.GetCustomerByEmail(request.Email), cancellationToken);
        if (customerExistedEmail != null)
        {
            return Result.Failed<int>("Email đã được sử dụng tài khoản khác.");
        }

        var entity = _mapper.Map<Domain.Entities.Customers.Customer>(request);
        entity.UID = Guid.NewGuid();
        var salt = BCrypt.Net.BCrypt.GenerateSalt(10);
        if (string.IsNullOrEmpty(request.Password)) request.Password = $"{request.Phone}@{DateTime.Now.Year}";
        entity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, salt, false, HashType.SHA512);

        if (!string.IsNullOrEmpty(request.AddressLine) || request.ProvinceId > 0)
        {
            var address = await _context.Addresses.FirstOrDefaultAsync(AddressExpression.IsDuplicated(request.AddressLine, request.ProvinceId.Value, request.DistrictId.Value, request.WardId.Value), cancellationToken);
            if (address == null)
            {
                var newAddressEntity = _mapper.Map<Address>(request);
                newAddressEntity.CountryId = (int)AddressSystemIds.VN;
                await _context.Addresses.AddAsync(newAddressEntity, cancellationToken);

                entity.CustomerAddresses.Add(new CustomerAddress()
                {
                    FullName = request.FirstName.DisplayFullName(request.LastName),
                    Phone = request.Phone,
                    AddressId = newAddressEntity.Id
                });
            }
            else
            {
                entity.CustomerAddresses.Add(new CustomerAddress()
                {
                    FullName = request.FirstName.DisplayFullName(request.LastName),
                    Phone = request.Phone,
                    AddressId = address.Id
                });
            }
        }

        await _context.Customers.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success(entity.Id);
    }
}