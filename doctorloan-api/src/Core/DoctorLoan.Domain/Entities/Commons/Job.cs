
using System.ComponentModel.DataAnnotations.Schema;
using DoctorLoan.Domain.Interfaces;

namespace DoctorLoan.Domain.Entities.Commons;

[Table("Jobs")]
public class Job : BaseEntityAudit<int>, ISoftDeleteEntity
{
    public string Title { get; set; }
    public bool IsDelete { get; set; }
}