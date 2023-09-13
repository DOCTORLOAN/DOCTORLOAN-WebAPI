using DoctorLoan.Domain.Enums.Commons;

namespace DoctorLoan.Products.Application.Features.Products.Dtos;
public class ProductFilterResultDto
{
  
    public int Id { get; set; }
    public string Name { get; set; }=string.Empty;
    public string Sku { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public List<string> CategoryNames { get; set; } = new List<string>();
    public StatusEnum  Status { get; set; }
    public string BranchName { get; set; }=string.Empty;
    public int AvaiableStock { get; set; }
    public int Stock { get; set; }
    public DateTimeOffset? LastModified { get; set; }
    public List<ProductItemDto> ProductItems { get; set; }=new List<ProductItemDto>();

}
