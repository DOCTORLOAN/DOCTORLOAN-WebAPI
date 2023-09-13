using DoctorLoan.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Users;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<User, int>();

        builder.HasIndex(x => new { x.Code, x.Phone }).IsUnique();

        builder.Property(x => x.FullName).IsRequired();
        builder.Property(x => x.Status).IsRequired();
        builder.Property(x => x.PasswordHash).IsRequired();
        builder.Property(x => x.SourcePlatform).IsRequired();
        builder.Property(e => e.ParentTreeId).HasColumnName("ParentTreeId").HasColumnType("ltree");

        builder.HasMany(s => s.UserMedias)
         .WithOne()
         .HasForeignKey(s => s.UserId)
         .OnDelete(DeleteBehavior.Restrict)
         .IsRequired(false);

        builder.HasOne(s => s.Role)
            .WithMany(s => s.Users)
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
    }
}
