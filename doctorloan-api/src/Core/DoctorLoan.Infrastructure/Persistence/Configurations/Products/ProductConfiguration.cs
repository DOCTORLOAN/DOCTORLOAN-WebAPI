using DoctorLoan.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Products;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<Product, int>();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(250).IsUnicode();
        builder.Property(x => x.Slug).IsRequired().HasMaxLength(500);
        builder.Property(x => x.Sku).IsRequired().HasMaxLength(50);
        builder.HasQueryFilter(x => !x.IsDelete);

        builder.HasMany(x => x.ProductDetails)
           .WithOne(x => x.Product)
           .HasForeignKey(x => x.ProductId)
           .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.ProductAttributes)
           .WithOne(x => x.Product)
           .HasForeignKey(x => x.ProductId)
           .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Brand)
         .WithMany()
         .HasForeignKey(x => x.BrandId)
         .IsRequired();
        
        
    }
}
