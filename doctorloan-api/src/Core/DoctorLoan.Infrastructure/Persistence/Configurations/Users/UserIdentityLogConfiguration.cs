using DoctorLoan.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Users;

public class UserIdentityLogConfiguration : IEntityTypeConfiguration<UserIdentityLog>
{
    public void Configure(EntityTypeBuilder<UserIdentityLog> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<UserIdentityLog, int>();

        builder.Property(x => x.UserIdentityId).IsRequired();
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.Type).IsRequired();
        builder.Property(x => x.IdentityNo).IsRequired();
        builder.Property(x => x.Status).IsRequired();

        builder.HasOne(s => s.UserIdentity)
           .WithMany(s => s.UserIdentityLogs)
           .HasForeignKey(a => a.UserIdentityId)
           .OnDelete(DeleteBehavior.Restrict);
    }
}
