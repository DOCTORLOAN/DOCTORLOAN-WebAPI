namespace DoctorLoan.Application.Models.Settings;

public class SystemConfiguration
{
    public int UserCodeLength { get; set; }
    public string DefaultPrefixCode { get; set; }
    public string EncryptionKey { get; set; }
    public int TimeExpired { get; set; }
    public string AllowCORSUrl { get; set; }
    public string DefaultPassword { get; set; }
    public string[] GetAllowCORS() => AllowCORSUrl?.Split(',');
    public int DepartmentSellerId { get; set; }
}
