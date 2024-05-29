using DoctorLoan.Domain.Entities.MedicalRecord;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.MedicalRecord;
public class SymptomGroupsConfiguration : IEntityTypeConfiguration<SymptomGroups>
{
    public void Configure(EntityTypeBuilder<SymptomGroups> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<SymptomGroups, int>();
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(70)
            .IsUnicode();
    }
}
