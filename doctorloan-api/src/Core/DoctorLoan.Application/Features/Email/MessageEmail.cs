using MimeKit;

namespace DoctorLoan.Application.Features.Email;
public class MessageEmail
{
    public List<MailboxAddress> To { get; set; }

    public string Subject { get; set; }

    public string Content { get; set; }

    public MessageEmail(List<ToInfo> to, string subject, string content)
    {
        To = new List<MailboxAddress>();
        To.AddRange(to.Select(x => new MailboxAddress(x.Name, x.Mail)));
        Subject = subject;
        Content = content;
    }
}

public class ToInfo
{
    public string Name { get; set; }

    public string Mail { get; set; }
}
