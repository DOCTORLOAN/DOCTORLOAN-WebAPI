using DoctorLoan.Domain.Entities.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Settings;

public class SettingUserConfiguration : IEntityTypeConfiguration<SettingUser>
{
    public void Configure(EntityTypeBuilder<SettingUser> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<SettingUser, int>();

        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.Type).IsRequired();

        builder.HasOne(x => x.Users)
        .WithMany()
        .HasForeignKey(x => x.UserId)
        .OnDelete(DeleteBehavior.Restrict)
        .IsRequired(true);

        builder.HasMany(s => s.SettingUserLogs)
       .WithOne()
       .HasForeignKey(s => s.SettingUserId)
       .OnDelete(DeleteBehavior.Cascade)
       .IsRequired(true);
    }
}