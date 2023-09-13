using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Domain.Entities.Emails;
using DoctorLoan.Domain.Enums.Emails;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.Application.Features.Email;

public class SendEmailHandle : ApplicationBaseService<SendEmailHandle>, IRequestHandler<SendEmail, bool>
{
    public IEmailSenderService EmailSender;

    public SendEmailHandle(ILogger<SendEmailHandle> logger,
        IApplicationDbContext context,
        IEmailSenderService emailSender,
        ICurrentRequestInfoService currentRequestInfoService,
        ICurrentTranslateService currentTranslateService,
        IDateTime dateTime)
        : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
        EmailSender = emailSender;
    }

    public async Task<bool> Handle(SendEmail request, CancellationToken cancellationToken)
    {
        bool result;
        try
        {
            var rng = new Random();
            var message = new MessageEmail(request.To, request.Subject, request.Content);
            var logRequest = new EmailRequest
            {
                Code = request.Content,
                Email = string.Join(",", request.To),
                Type = EmailType.None
            };
            result = await EmailSender.SendEmail(message, logRequest, cancellationToken);
        }
        catch (Exception)
        {
            result = false;
        }

        return result;
    }
}
