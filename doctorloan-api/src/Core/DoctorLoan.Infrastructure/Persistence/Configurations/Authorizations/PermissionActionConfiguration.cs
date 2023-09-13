using DoctorLoan.Domain.Entities.Authorizations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Authorizations;

public class PermissionActionConfiguration : IEntityTypeConfiguration<PermissionAction>
{
    public void Configure(EntityTypeBuilder<PermissionAction> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();

        builder.HasIndex(x => new { x.ModuleId, x.ActionId }).IsUnique();

        builder.HasQueryFilter(x => !x.IsDelete);
        builder.Property(x => x.ModuleId).IsRequired();
        builder.Property(x => x.ActionId).IsRequired();
    }
}

