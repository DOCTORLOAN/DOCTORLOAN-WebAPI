using MediatR;

namespace DoctorLoan.Application.Features.Email;
public class SendEmail : IRequest<bool>
{
    public List<ToInfo> To { get; set; }
    public string Content { get; set; }
    public string Subject { get; set; }
}
