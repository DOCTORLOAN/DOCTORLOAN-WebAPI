using DoctorLoan.Domain.Entities.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Settings;

public class SettingAppContentConfiguration : IEntityTypeConfiguration<SettingAppContent>
{
    public void Configure(EntityTypeBuilder<SettingAppContent> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<SettingAppContent, int>();

        builder.Property(x => x.SettingAppId).IsRequired();
        builder.Property(x => x.Language).IsRequired();
        builder.Property(x => x.Title).IsRequired();
        builder.Property(x => x.IsActive).IsRequired();
    }
}