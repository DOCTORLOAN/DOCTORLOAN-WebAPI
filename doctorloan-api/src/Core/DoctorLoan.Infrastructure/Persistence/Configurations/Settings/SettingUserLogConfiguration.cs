using DoctorLoan.Domain.Entities.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Settings;

public class SettingUserLogConfiguration : IEntityTypeConfiguration<SettingUserLog>
{
    public void Configure(EntityTypeBuilder<SettingUserLog> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<SettingUserLog, int>();

        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.Type).IsRequired();
        builder.Property(x => x.SettingUserId).IsRequired();
    }
}