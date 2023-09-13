using DoctorLoan.Domain.Entities.News;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Products;
public class NewsCategoryMappingConfiguration : IEntityTypeConfiguration<NewsCategoryMapping>
{
    public void Configure(EntityTypeBuilder<NewsCategoryMapping> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<NewsCategoryMapping, int>();
        builder.HasOne(x => x.NewsCategory)
            .WithMany()
            .HasForeignKey(x => x.NewsCategoryId)
            .IsRequired();
        builder.HasOne(x => x.NewsItem)
            .WithMany(x=>x.NewsCategories)
            .HasForeignKey(x => x.NewsItemId)
            .IsRequired();

    }
}