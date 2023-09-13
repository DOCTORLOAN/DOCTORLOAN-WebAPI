using DoctorLoan.Application.Common.Mappings;
using DoctorLoan.Domain.Entities.Users;
using DoctorLoan.Domain.Enums.Commons;
using DoctorLoan.Domain.Enums.Users;
using Newtonsoft.Json;

namespace DoctorLoan.User.Application.Features.Users.Dtos;
public class UserDto : IMapFrom<Domain.Entities.Users.User>
{
    public int Id { get; set; }

    public string Code { get; set; }

    public int? RoleId { get; set; }

    public string RoleName { get; set; }

    public string FullName { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string Phone { get; set; }

    public UserStatus Status { get; set; }

    public Gender Gender { get; set; }

    public bool IsResetPassword { get; set; }

    public bool IsSignOut { get; set; }

    public DateTimeOffset? LastSignIn
    {
        get
        {
            return this.UserActivities?.FirstOrDefault()?.Created;
        }
    }

    [JsonIgnore]
    public List<UserActivity>? UserActivities { get; set; }
}

