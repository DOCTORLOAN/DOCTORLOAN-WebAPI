using DoctorLoan.Domain.Enums.Commons;

namespace DoctorLoan.News.Application.Features.NewsCategories.Dtos;
public class NewsCategoryDto
{
    
    public int Id { get; set; }
    public int? ParentId { get; set; }
    public string Name { get; set; }=string.Empty;
    public StatusEnum Status { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Sort { get; set; }
    public DateTimeOffset LastModified { get; set; }

}
