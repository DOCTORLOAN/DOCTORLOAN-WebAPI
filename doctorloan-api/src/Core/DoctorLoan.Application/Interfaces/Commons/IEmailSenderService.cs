using DoctorLoan.Application.Features.Email;
using DoctorLoan.Domain.Entities.Emails;

namespace DoctorLoan.Application.Interfaces.Commons;
public interface IEmailSenderService
{
    Task<bool> SendEmail(MessageEmail message, EmailRequest request, CancellationToken cancellationToken = default);

    Task<string> GenerateOTP(CancellationToken cancellationToken = default);
}
