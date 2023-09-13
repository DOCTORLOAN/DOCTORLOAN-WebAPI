namespace DoctorLoan.Application.Models.Commons;
public abstract class BasePagableModel
{
    public int PageNumber { get; set; }
    public int PageIndex => PageNumber - 1 >= 0 ? PageNumber - 1 : 0;
    public int PageSize { get; set; }
}
