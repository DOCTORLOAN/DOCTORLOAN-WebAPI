using DoctorLoan.Products.Application.Features.Products.Portal.Dtos;

namespace DoctorLoan.Products.Application.Features.Categories.Portal.Dtos;
public class GetCategoriesResultDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public List<GetCategoriesResultDto> Childs { get; set; }=new List<GetCategoriesResultDto>();
    public List<SearchProductResultDto> Products { get; set; } = new List<SearchProductResultDto>();
}
