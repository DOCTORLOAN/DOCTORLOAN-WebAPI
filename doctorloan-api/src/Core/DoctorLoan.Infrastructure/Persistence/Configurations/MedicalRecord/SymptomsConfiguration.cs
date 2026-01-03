using DoctorLoan.Domain.Entities.MedicalRecord;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.MedicalRecord;
public class SymptomsConfiguration : IEntityTypeConfiguration<Symptoms>
{
    public void Configure(EntityTypeBuilder<Symptoms> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<Symptoms, int>();
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(70)
            .IsUnicode();
        builder.HasOne(x => x.SymptomGroup)
            .WithMany(x => x.Symptoms)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}