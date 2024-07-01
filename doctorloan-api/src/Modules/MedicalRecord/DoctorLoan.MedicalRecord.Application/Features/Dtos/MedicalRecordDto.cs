using DoctorLoan.Application.Common.Mappings;
using DoctorLoan.Domain.Enums.Commons;

namespace DoctorLoan.MedicalRecord.Application.Features.Dtos;
public class MedicalRecordDto : IMapFrom<Domain.Entities.MedicalRecord.MedicalRecord>
{
    public string MedicalRecordNo { get; set; }
    public string FullName { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public DateTimeOffset? DateCreated { get; set; }
    public StatusEnum Status { get; set; }
    public string OtherMedicalHistory { get; set; }
    public int ParentId { get; set; }
    public string Noted { get; set; }
}
