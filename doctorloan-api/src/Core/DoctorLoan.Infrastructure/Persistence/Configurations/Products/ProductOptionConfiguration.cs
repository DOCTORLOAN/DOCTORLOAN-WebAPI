using DoctorLoan.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Products;

public class ProductOptionConfiguration : IEntityTypeConfiguration<ProductOption>
{
    public void Configure(EntityTypeBuilder<ProductOption> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<ProductOption, int>();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(70).IsUnicode();
        builder.Property(x => x.DisplayValue).HasMaxLength(70);
    }
}
