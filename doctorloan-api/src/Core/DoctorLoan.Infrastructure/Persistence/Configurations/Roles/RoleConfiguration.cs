using DoctorLoan.Domain.Entities.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Configurations;

public class BrandConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<Role, int>();

        builder.Property(x => x.DepartmentId).IsRequired();
        builder.HasIndex(x => x.Code).IsUnique();
        builder.Property(x => x.Name).IsRequired();

        builder.HasOne(s => s.Department)
        .WithMany(s => s.DepartmentRoles)
        .HasForeignKey(b => b.DepartmentId)
        .OnDelete(DeleteBehavior.Restrict);
    }
}
