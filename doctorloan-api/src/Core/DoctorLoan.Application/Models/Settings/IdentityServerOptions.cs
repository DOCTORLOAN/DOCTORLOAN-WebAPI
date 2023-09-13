namespace DoctorLoan.Application.Models.Settings;

public class IdentityServerOptions
{
    public string Authority { get; internal set; }

    public string RequireHttpsMetadata { get; internal set; }
}