using DoctorLoan.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Products;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<Category, int>();
        builder.Property(x=>x.ParentId).HasColumnType("ltree");
        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(150)
            .IsUnicode();
        builder.Property(x => x.Slug)
        .IsRequired()
        .HasMaxLength(255);
    }
}
