using DoctorLoan.Domain.Entities.MedicalRecord;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.MedicalRecord;
public class MedicalRecordsSymptomsConfiguration : IEntityTypeConfiguration<MedicalRecordsSymptoms>
{
    public void Configure(EntityTypeBuilder<MedicalRecordsSymptoms> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<MedicalRecordsSymptoms, int>();

        builder.HasOne(x => x.MedicalRecord)
            .WithMany(x => x.MedicalRecordsSymptoms)
            .HasForeignKey(x => x.MedicalRecordId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Symptom)
            .WithMany()
            .HasForeignKey(x => x.SymptomId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}