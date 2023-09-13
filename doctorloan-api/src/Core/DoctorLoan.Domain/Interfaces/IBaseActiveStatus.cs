using DoctorLoan.Domain.Enums.Commons;

namespace DoctorLoan.Domain.Interfaces;
public interface IBaseActiveStatus
{
    BaseEntityStatus Status { get; set; }
}
