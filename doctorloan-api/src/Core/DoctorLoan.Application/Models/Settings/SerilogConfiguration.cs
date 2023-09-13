namespace DoctorLoan.Application.Models.Settings;
public class SerilogConfiguration
{
    public string URL { get; set; }
    public string ApiKey { get; set; }
    public string Source { get; set; }
    public bool EnableRequestResponseLogging { get; set; } = false;
    public int MinimumLevel_System { get; set; } = 3;
    public int MinimumLevel_Microsoft { get; set; } = 3;
    public int MinimumLevel_Microsoft_AspNetCore { get; set; } = 3;
}
