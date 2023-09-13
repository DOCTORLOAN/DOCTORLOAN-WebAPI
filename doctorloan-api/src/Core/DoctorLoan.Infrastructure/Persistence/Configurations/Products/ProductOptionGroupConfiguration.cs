using DoctorLoan.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Products;

public class ProductOptionGroupConfiguration : IEntityTypeConfiguration<ProductOptionGroup>
{
    public void Configure(EntityTypeBuilder<ProductOptionGroup> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<ProductOptionGroup, int>();
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(70)
            .IsUnicode();

    }
}
