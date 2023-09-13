using DoctorLoan.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Users;

public class UserActivityConfiguration : IEntityTypeConfiguration<UserActivity>
{
    public void Configure(EntityTypeBuilder<UserActivity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<UserActivity, int>();

        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.ActivityType).IsRequired();


        builder.HasOne(s => s.User)
         .WithMany(x => x.UserActivities)
         .HasForeignKey(s => s.UserId)
         .OnDelete(DeleteBehavior.Cascade)
         .IsRequired();
    }
}