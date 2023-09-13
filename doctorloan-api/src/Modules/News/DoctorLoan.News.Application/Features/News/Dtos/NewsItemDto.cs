using DoctorLoan.Domain.Enums.Commons;

namespace DoctorLoan.News.Application.Features.News.Dtos;
public class NewsItemDto
{
    public int Id { get; set; }
    public string Title { get; set; }=string.Empty;
    public StatusEnum Status { get; set; }
    public List<NewsItemDetailDto> NewsItemDetails { get; set; } =new List<NewsItemDetailDto>();
    public virtual List<NewsMediaDto> NewsMedias { get; set; } = new List<NewsMediaDto>();
    public List<TagDto> Tags { get; set; } = new List<TagDto>();
    public List<int> CategoryIds { get; set; } = new List<int>();
    public DateTimeOffset Created { get; set; }

}
public class NewsItemDetailDto 
{
    public int Id { get; set; }   
    public int LanguageId { get; set; }
    public string? Title { get; set; }
    public string? Short { get; set; }
    public string? Full { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaKeyword { get; set; }
    public string? MetaDescription { get; set; }

}
public class NewsMediaDto 
{
    public string MediaUrl { get; set; } = string.Empty;
    public int Id { get; set; }
    public bool IsThumb { get; set; }   
    public int MediaId { get; set; }
    public int OrderBy { get; set; }
}
public class TagDto
{
    public int Id { get; set; }
    public  string? Name { get; set; }
}