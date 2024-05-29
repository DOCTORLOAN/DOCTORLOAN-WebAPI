using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DoctorLoan.Domain.Entities.MedicalRecord;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.MedicalRecord;
public class MedicalRecordsCategoryMappingConfiguration : IEntityTypeConfiguration<MedicalRecordsCategoryMapping>
{
    public void Configure(EntityTypeBuilder<MedicalRecordsCategoryMapping> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<MedicalRecordsCategoryMapping, int>();

        builder.HasOne(x => x.MedicalRecord)
            .WithMany(x => x.MedicalRecordsCategoryMapping)
            .HasForeignKey(x => x.MedicalRecordId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.MedicalRecordsCategory)
            .WithMany(x => x.MedicalRecordsCategoryMappings)
            .HasForeignKey(x => x.MedicalRecordsCategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}