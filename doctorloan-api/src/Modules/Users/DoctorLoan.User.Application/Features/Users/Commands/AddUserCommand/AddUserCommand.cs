using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Domain.Enums.Commons;
using DoctorLoan.Domain.Enums.Users;
using MediatR;

namespace DoctorLoan.User.Application.Features.Users;
public class AddUserCommand : IRequest<Result>
{
    public string Code { get; set; }
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Password { get; set; }

    private string _email;
    public string Email
    {
        get
        {
            return string.IsNullOrEmpty(_email) ? string.Empty : _email.Trim();
        }
        set { _email = value; }
    }

    private string _phone;
    public string Phone
    {
        get => _phone.RemoveFirstZero();
        set { _phone = value; }
    }

    public Gender Gender { get; set; }
    public int? Avatar { get; set; }
    public UserStatus? Status { get; set; }

    public DateTime? DOB { get; set; }

}
