namespace DoctorLoan.Application.Models.Users;

public class CurrentUserInfo
{
    public CurrentUserInfo()
    {

    }

    public int Id { get; set; }
    public string FullName { get; set; }
    public int RoleId { get; set; }
    public bool IsSignOut { get; set; }
    public int? ValidUnixTime { get; set; }
}
