using DoctorLoan.Domain.Entities.Banks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Banks;

public class BranchConfiguration : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<Branch, int>();

        builder.HasQueryFilter(x => !x.IsDelete);
        builder.Property(x => x.Name).IsRequired();
    }
}