using DoctorLoan.Domain.Entities.Products;

namespace DoctorLoan.Products.Application.Features.Products.Dtos;
public class CategoryOptionDto
{
    public CategoryOptionDto(Category category)
    {
        Name = category.Name;
        Id = category.Id;
    }
    public string Name { get; set; }
    public int Id { get; set; }

}
