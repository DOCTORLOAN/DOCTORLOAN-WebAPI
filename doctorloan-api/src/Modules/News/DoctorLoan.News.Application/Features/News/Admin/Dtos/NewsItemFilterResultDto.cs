using DoctorLoan.Domain.Enums.Commons;

namespace DoctorLoan.News.Application.Features.News.Admin.Dtos;
public class NewsItemFilterResultDto
{
    public string ImageUrl { get; set; } = string.Empty;
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public StatusEnum Status { get; set; }   
    public List<string> Tags { get; set; } = new List<string>();
    public List<string> Categories { get; set; } = new List<string>();
    public DateTimeOffset LastModified {  get; set; }
}
