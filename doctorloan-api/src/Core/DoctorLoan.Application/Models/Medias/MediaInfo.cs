using DoctorLoan.Domain.Enums.Medias;

namespace DoctorLoan.Application.Models.Medias;

public class MediaInfo
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string Path { get; set; }

    public MediaType Type { get; set; }
}