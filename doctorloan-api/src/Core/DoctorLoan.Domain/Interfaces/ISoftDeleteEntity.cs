namespace DoctorLoan.Domain.Interfaces;
public partial interface ISoftDeleteEntity
{
    bool IsDelete { get; set; }
}
