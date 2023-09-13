using Newtonsoft.Json;

namespace DoctorLoan.Application.Models.Authenticates;
public class JWTTokens
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }

    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }

    [JsonProperty("expires_on")]
    public long ExpiresOn { get; set; }

    [JsonProperty("refesh_token")]
    public string RefeshToken { get; set; }
}
