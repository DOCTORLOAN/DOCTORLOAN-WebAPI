using DoctorLoan.Application.Models.Commons;

namespace DoctorLoan.Products.Application.Features.Products.Dtos;
public class ListProductOptions
{
    public List<Option> Categories { get; set; }=new List<Option>();
    public List<Option> OptionGroups { get; set; } = new List<Option>();
    public List<Option> Brands { get; set; } = new List<Option>();
    public List<Option> Attributes { get;  set; } = new List<Option>();
}
