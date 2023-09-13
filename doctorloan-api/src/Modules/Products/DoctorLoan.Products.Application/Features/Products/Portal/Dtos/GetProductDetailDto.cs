using DoctorLoan.Products.Application.Features.Products.Dtos;

namespace DoctorLoan.Products.Application.Features.Products.Portal.Dtos;
public class GetProductDetailDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Sku { get; set; }
    public required ProductDetailDto ProductDetail { get; set; }
    public List<GetProductDetailProductItemDto> ProductItems { get; set; } = new List<GetProductDetailProductItemDto>();
    public List<GetProductDetaiMediaDto> ProductMedias { get; set; } = new List<GetProductDetaiMediaDto>();
    public List<int> CategoryIds { get; set; } = new List<int>();

}
public class GetProductDetailProductItemDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Sku { get; set; }
    public decimal Price { get; set; }
    public decimal PriceDiscount { get; set; }  
    public List<GetProductDetailProductOptionDto> ProductOptions { get; set; } = new List<GetProductDetailProductOptionDto>();
}
public class GetProductDetailProductOptionDto
{
    public int OptionGroupId { get; set; }
    public required string GroupName { get; set; }
    public required string Name { get; set; }
    public string? DisplayValue { get; set; }
}
public class GetProductDetaiMediaDto
{
    public required string MediaUrl { get; set; }
    public int ProductItemId { get; set; }
}
