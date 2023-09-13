using DoctorLoan.Domain.Enums.Commons;
using DoctorLoan.Domain.Enums.Contents;

namespace DoctorLoan.Contents.Application.Features.Contents.Dtos;

public class ContentDto
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public StatusEnum Status { get; set; }
    public string StatusName { get; set; }
    public ContentTypeEnum Type { get; set; }
    public string TypeName { get; set; }
}
