using DoctorLoan.Domain.Entities.Authorizations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Authorizations;

public class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
{
    public void Configure(EntityTypeBuilder<UserPermission> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<UserPermission, long>();

        builder.HasIndex(x => new { x.UserId, x.PermissionActionId }).IsUnique();

        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.PermissionActionId).IsRequired();

        builder.HasOne(x => x.User)
             .WithMany(x => x.UserPermissions)
             .HasForeignKey(x => x.UserId)
             .IsRequired()
             .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.PermissionAction)
            .WithMany(x => x.UserPermissions)
            .HasForeignKey(x => x.PermissionActionId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}

