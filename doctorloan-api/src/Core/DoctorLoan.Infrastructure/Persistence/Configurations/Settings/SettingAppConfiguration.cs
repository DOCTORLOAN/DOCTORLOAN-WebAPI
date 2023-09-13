using DoctorLoan.Domain.Entities.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Settings;


public class SettingAppConfiguration : IEntityTypeConfiguration<SettingApp>
{
    public void Configure(EntityTypeBuilder<SettingApp> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<SettingApp, int>();

        builder.Property(x => x.Type).IsRequired();
        builder.Property(x => x.PrefixCode).IsRequired();
        builder.Property(x => x.IsActive).IsRequired();

        builder.HasMany(s => s.SettingAppContents)
           .WithOne(s => s.SettingApp)
           .HasForeignKey(b => b.SettingAppId)
           .OnDelete(DeleteBehavior.Restrict);
    }
}