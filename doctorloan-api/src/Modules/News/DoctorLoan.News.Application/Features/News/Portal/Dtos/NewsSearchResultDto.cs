namespace DoctorLoan.News.Application.Features.News.Portal.Dtos;
public class NewsSearchResultDto
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string ImageUrl { get; set; }
    public required string Slug { get; set; }
    public string Short { get; set; }
    public DateTimeOffset Created { get; set; }
}
