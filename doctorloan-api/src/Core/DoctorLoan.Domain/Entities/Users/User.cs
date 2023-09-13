using System.ComponentModel.DataAnnotations.Schema;
using DoctorLoan.Domain.Entities.Authorizations;
using DoctorLoan.Domain.Entities.Roles;
using DoctorLoan.Domain.Enums.Commons;
using DoctorLoan.Domain.Enums.Users;
using Microsoft.EntityFrameworkCore;

namespace DoctorLoan.Domain.Entities.Users;

[Table("Users")]
public class User : BaseEntityAudit<int>
{
    public Guid UUId { get; set; }

    public int RoleId { get; set; }

    public string Code { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string FullName { get; set; }

    public string UserName { get; set; }

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
        get
        {
            return string.IsNullOrEmpty(_phone) ? string.Empty : _phone.Trim();
        }
        set { _phone = value; }
    }

    public int? ParentId { get; set; }

    public LTree? ParentTreeId { get; set; }

    public UserStatus Status { get; set; }

    public LanguageEnum LanguageId { get; set; }

    public Gender Gender { get; set; }

    public DateTime? DOB { get; set; }

    public string PasswordHash { get; set; }

    public string Remarks { get; set; }

    public SourcePlatform SourcePlatform { get; set; }

    public bool IsResetPassword { get; set; } = false;

    public bool IsSignOut { get; set; } = false;

    public int? ValidUnixTime { get; set; }


    public virtual Role Role { get; set; }
    public virtual User ParentUser { get; set; }
    public virtual UserDetail UserDetail { get; set; }

    public virtual ICollection<UserDevice> UserDevices { get; set; } = new List<UserDevice>();
    public virtual ICollection<UserBankBranch> UserBankBranchs { get; set; } = new List<UserBankBranch>();
    public virtual ICollection<UserIdentity> UserIdentities { get; set; } = new List<UserIdentity>();
    public virtual ICollection<UserMedia> UserMedias { get; set; } = new List<UserMedia>();
    public virtual ICollection<UserAddress> UserAddresses { get; set; } = new List<UserAddress>();
    public virtual ICollection<UserActivity> UserActivities { get; set; } = new List<UserActivity>();
    public virtual ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
}
