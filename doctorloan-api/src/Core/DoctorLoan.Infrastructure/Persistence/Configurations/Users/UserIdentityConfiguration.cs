using DoctorLoan.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Users;

public class UserIdentityConfiguration : IEntityTypeConfiguration<UserIdentity>
{
    public void Configure(EntityTypeBuilder<UserIdentity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<UserIdentity, int>();

        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.Type).IsRequired();
        builder.Property(x => x.DOB).IsRequired();
        builder.Property(x => x.IdentityNo).IsRequired();
        builder.HasIndex(x => x.IdentityNo).IsUnique().HasFilter(@"""Status"" = 1 OR ""Status"" = 2");
        builder.Property(x => x.IssuedDate).IsRequired();
        builder.Property(x => x.PlaceOfIssue).IsRequired();
        builder.Property(x => x.Status).IsRequired();
        builder.Property(x => x.Gender).IsRequired();

        builder.HasMany(s => s.UserMedias)
           .WithOne(s => s.UserIdentity)
           .HasForeignKey(a => a.UserIdentityId)
           .OnDelete(DeleteBehavior.Restrict)
           .IsRequired(false);
    }
}