using DoctorLoan.Domain.Entities.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Settings;


public class SettingConfiguration : IEntityTypeConfiguration<Setting>
{
    public void Configure(EntityTypeBuilder<Setting> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<Setting, int>();
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Value).IsRequired();
        builder.Property(x => x.SystemId).IsRequired();

    }
}