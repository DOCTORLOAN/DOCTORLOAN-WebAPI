namespace DoctorLoan.Products.Application.Features.Products.Portal.Dtos;
public class SearchProductResultDto
{    
    public int Id { get; set; }
    public required string Sku { get; set; }
    public required string Name { get; set; }
    public  string? ImageUrl { get; set; }
    public  string? BrandName { get; set; }
    public decimal Price { get; set; }
    public decimal PriceDiscount { get; set; }
    public required string Slug { get; set; }
    public string Short { get; set; }=string.Empty;
}
