using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Features.Email;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Domain.Entities.Emails;
using DoctorLoan.Domain.Enums.Emails;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace DoctorLoan.Infrastructure.Services;
public class EmailSenderServices : IEmailSenderService
{
    protected readonly IApplicationDbContext _context;
    private readonly IOptions<EmailConfiguration> _emailConfig;
    protected readonly ILogger<EmailSenderServices> _logger;

    public EmailSenderServices(IApplicationDbContext context, IOptions<EmailConfiguration> emailConfig,
        ILogger<EmailSenderServices> logger)
    {
        _context = context;
        _emailConfig = emailConfig;
        _logger = logger;
    }

    public async Task<bool> SendEmail(MessageEmail message, EmailRequest request, CancellationToken cancellationToken = default)
    {
        var emailMessage = CreateEmailMessage(message);
        var result = await Send(emailMessage, request);
        return result;
    }

    public async Task<string> GenerateOTP(CancellationToken cancellationToken = default)
    {
        string rs = string.Empty;
        bool isLoop = true;

        while (isLoop)
        {
            var random = DataExtensions.GetUniqueKey(6);
            var existedCode = await _context.EmailRequests.AnyAsync(s => s.Code == random && s.Type == EmailType.OTP && !s.Expired, cancellationToken);
            if (!existedCode)
            {
                rs = random;
                isLoop = false;
            }
        }
        return rs;
    }


    private MimeMessage CreateEmailMessage(MessageEmail message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(_emailConfig.Value.DisplayName, _emailConfig.Value.From));
        emailMessage.To.AddRange(message.To);
        emailMessage.Subject = message.Subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };
        return emailMessage;
    }

    private Task<bool> Send(MimeMessage mailMessage, EmailRequest request, CancellationToken cancellationToken = default)
    {
        var result = false;
        using (var client = new SmtpClient())
        {
            try
            {
                client.Connect(_emailConfig.Value.SmtpServer, _emailConfig.Value.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfig.Value.UserName, _emailConfig.Value.Password);
                client.Send(mailMessage);
                result = true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Send email OTP failed at: {DateTimeOffset.Now}. \n Reason: {ex.Message}");

            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }
        return Task.FromResult(result);
    }
}
