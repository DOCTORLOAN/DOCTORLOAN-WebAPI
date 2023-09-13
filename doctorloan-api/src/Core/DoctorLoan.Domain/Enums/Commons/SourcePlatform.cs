using System.ComponentModel;

namespace DoctorLoan.Domain.Enums.Commons;

public enum SourcePlatform
{
    /// <summary>
    /// Nguồn từ Landing Page
    /// </summary>
    [Description("Landing Page")]
    LandingPage = 1,

    /// <summary>
    /// Nguồn từ Web Admin
    /// </summary>
    [Description("Admin")]
    WebAdmin = 2,

    /// <summary>
    /// Unknow
    /// </summary>
    [Description("Unknown")]
    Unknown = -1
}