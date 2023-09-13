using Newtonsoft.Json;

namespace DoctorLoan.Domain.Entities.Users;

public class UserRefreshTokens
{
    public int Id { get; set; }

    public string UserName { get; set; }

    [JsonProperty("refesh_token")]
    public string RefreshToken { get; set; }

    public bool IsActive { get; set; } = true;
}