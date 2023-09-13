using System.ComponentModel.DataAnnotations;
using AutoMapper;
using DoctorLoan.Application;
using DoctorLoan.Application.Common.Expressions;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Features.Email;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Customer.Application.Commons.Expressions;
using DoctorLoan.Domain.Entities.Addresses;
using DoctorLoan.Domain.Entities.Customers;
using DoctorLoan.Domain.Entities.Emails;
using DoctorLoan.Domain.Enums.Addresses;
using DoctorLoan.Domain.Enums.Bookings;
using DoctorLoan.Domain.Enums.Emails;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.Booking.Application.Features.Commands;

public class AddBookingCommand : IRequest<Result<int>>
{
    public BookingType Type { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    private string _phone;
    public string Phone
    {
        get => _phone.RemoveFirstZero();
        set { _phone = value; }
    }

    public int? BookingTimes { get; set; }
    public DateOnly? BookingDate { get; set; }

    [DataType(DataType.Time)]
    public TimeOnly? BookingStartTime { get; set; }

    [DataType(DataType.Time)]
    public TimeOnly? BookingEndTime { get; set; }

    public string? AddressLine { get; set; }
    public int? ProvinceId { get; set; }
    public int? DistrictId { get; set; }
    public int? WardId { get; set; }
}

public class AddBookingCommandHandler : ApplicationBaseService<AddBookingCommandHandler>, IRequestHandler<AddBookingCommand, Result<int>>
{
    private readonly IMapper _mapper;
    private readonly IEmailSenderService _emailSender;

    public AddBookingCommandHandler(ILogger<AddBookingCommandHandler> logger, IApplicationDbContext context,
                                    ICurrentRequestInfoService currentRequestInfoService,
                                    IMapper mapper,
                                    IEmailSenderService emailSender,
                                    ICurrentTranslateService currentTranslateService,
                                    IDateTime dateTime)
    : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
        _mapper = mapper;
        _emailSender = emailSender;
    }

    public async Task<Result<int>> Handle(AddBookingCommand request, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(CustomerExpression.GetCustomerByPhone(request.Phone), cancellationToken);
        customer ??= new Domain.Entities.Customers.Customer
        {
            UID = Guid.NewGuid(),
            Phone = request.Phone,
            FirstName = request.FirstName,
            LastName = request.LastName,
            FullName = request.FirstName.DisplayFullName(request.LastName),
            Gender = Domain.Enums.Commons.Gender.Male,
        };

        if (customer.Id <= 0)
        {
            await _context.Customers.AddAsync(customer, cancellationToken);
        }

        var customerAddress = await HandleAddress(request, customer, cancellationToken);

        var newBooking = _mapper.Map<Domain.Entities.Bookings.Booking>(request);
        newBooking.Customer = customer;
        if (customerAddress is not null)
            newBooking.CustomerAddresses = customerAddress;
        newBooking.Status = Domain.Enums.Commons.BookingStatus.Pending;

        await _context.Bookings.AddAsync(newBooking, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        try
        {
            if (!string.IsNullOrEmpty(customer.Email))
            {
                var to = new List<ToInfo>() {
                new ToInfo() { Mail = customer.Email, Name = customer.FirstName.DisplayFullName(customer.LastName) }
            };
                var content = "Chúc mừng bạn đã đặt lịch thành công <br/>.";

                var message = new MessageEmail(to, "Đặt lịch thành công", content);
                var logRequest = new EmailRequest
                {
                    Code = content,
                    Email = string.Join(",", to),
                    Type = EmailType.None
                };

                _ = await _emailSender.SendEmail(message, logRequest, cancellationToken);

            }

        }
        catch
        {
            _logger.LogError($"Send email booking error with customer: {customer.Id}, booking = {newBooking.Id}");
        }

        return Result.Success(newBooking.Id);
    }

    private async Task<CustomerAddress> HandleAddress(AddBookingCommand request, Domain.Entities.Customers.Customer customer, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.AddressLine) && !request.ProvinceId.HasValue)
        {
            return null;
        }

        var address = await _context.Addresses.FirstOrDefaultAsync(AddressExpression.IsDuplicated(request.AddressLine, request.ProvinceId, request.DistrictId, request.WardId), cancellationToken);

        CustomerAddress customerAddress;
        if (address == null)
        {
            var newAddressEntity = new Address
            {
                AddressLine = request.AddressLine,
                ProvinceId = request.ProvinceId,
                DistrictId = request.DistrictId,
                WardId = request.WardId,
                CountryId = (int)AddressSystemIds.VN
            };

            await _context.Addresses.AddAsync(newAddressEntity, cancellationToken);

            customerAddress = new CustomerAddress()
            {
                Type = AddressType.Booking,
                FullName = request.FirstName.DisplayFullName(request.LastName),
                Phone = request.Phone,
                Address = newAddressEntity
            };
            customer.CustomerAddresses.Add(customerAddress);
        }
        else
        {
            customerAddress = new CustomerAddress()
            {
                Type = AddressType.Booking,
                FullName = request.FirstName.DisplayFullName(request.LastName),
                Phone = request.Phone,
                Address = address
            };
            customer.CustomerAddresses.Add(customerAddress);
        }
        return customerAddress;
    }
}