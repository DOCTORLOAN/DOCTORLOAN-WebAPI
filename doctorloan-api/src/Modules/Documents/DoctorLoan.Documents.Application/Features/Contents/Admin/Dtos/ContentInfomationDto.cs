using DoctorLoan.Contents.Application.Features.Contents.Dtos;
using DoctorLoan.Domain.Entities.Contents;
using DoctorLoan.Domain.Enums.Medias;

namespace DoctorLoan.Contents.Application.Features.Contents.Admin.Dtos;
public class ContentInfomationDto : ContentDto
{
    public ContentInfomationDto() { }

    public MediaInfoDto? Media { get; set; }
    public ContentInfomationDto(Content data)
    {
        Id = data.Id;
        Code = data.Code;
        Name = data.Name;
        Description = data.Description;
        Status = data.Status;
        Type = data.Type;
        TypeName = data.Type.ToString();
        StatusName = Status.ToString();
    }
}
public class MediaInfoDto
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string Path { get; set; }

    public MediaType Type { get; set; }

    public int? ContentId { get; set; }
}