using System.Text;
using AutoMapper;
using BCrypt.Net;
using DoctorLoan.Application;
using DoctorLoan.Application.Features.Email;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Domain.Entities.Emails;
using DoctorLoan.Domain.Enums.Emails;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.User.Application.Features.Users;


public record ForgetPasswordCommand(string Email) : IRequest<Result<bool>>;

public class ForgetPasswordCommandHandle : ApplicationBaseService<ForgetPasswordCommandHandle>, IRequestHandler<ForgetPasswordCommand, Result<bool>>
{

    private readonly IMapper _mapper;
    private readonly IEmailSenderService _emailSender;

    public ForgetPasswordCommandHandle(ILogger<ForgetPasswordCommandHandle> logger,
                            IMapper mapper,
                            IApplicationDbContext context,
                            ICurrentRequestInfoService currentRequestInfoService,
                            ICurrentTranslateService currentTranslateService,
                            IEmailSenderService emailSender,
                            IDateTime dateTime)
                            : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
        _mapper = mapper;
        _emailSender = emailSender;
    }

    public async Task<Result<bool>> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Email)) return Result.Failed<bool>(ServiceError.UserNotFound(_currentTranslateService));

        var user = await _context.Users.FirstOrDefaultAsync(s => s.Email.ToLower() == request.Email.Trim().ToLower(), cancellationToken);

        if (user == null) return Result.Failed<bool>(ServiceError.UserNotFound(_currentTranslateService));

        var salt = BCrypt.Net.BCrypt.GenerateSalt(10);
        var newPassword = RandomPassword();
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword, salt, false, HashType.SHA512);
        await _context.SaveChangesAsync(cancellationToken);

        var to = new List<ToInfo>() {
                    new ToInfo() { Mail = request.Email, Name = user.FullName }
        };
        var content = $"Mật khẩu mới của bạn là: <b style=\"color: #d39364;\">{newPassword}</b> <br/>. Cảm ơn bạn!";

        var message = new MessageEmail(to, "[DOCTORLOAN] Reset mật khẩu", content);
        var logRequest = new EmailRequest
        {
            Code = content,
            Email = string.Join(",", to),
            Type = EmailType.None
        };

        _ = await _emailSender.SendEmail(message, logRequest, cancellationToken);

        return Result.Success(true);
    }


    // Generate a random number between two numbers
    public int RandomNumber(int min, int max)
    {
        Random random = new Random();
        return random.Next(min, max);
    }
    public string RandomString(int size, bool lowerCase)
    {
        StringBuilder builder = new StringBuilder();
        Random random = new Random();
        char ch;
        for (int i = 0; i < size; i++)
        {
            ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
            builder.Append(ch);
        }
        if (lowerCase)
            return builder.ToString().ToLower();
        return builder.ToString();
    }

    public string RandomPassword()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(RandomString(4, true));
        builder.Append(RandomNumber(1000, 9999));
        builder.Append(RandomString(2, false));
        builder.Append('@' + DateTime.Now.ToString("yy"));
        return builder.ToString();
    }
}

