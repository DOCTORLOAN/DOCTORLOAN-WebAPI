using DoctorLoan.Domain.Entities.News;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Products;

public class NewsCategoryConfiguration : IEntityTypeConfiguration<NewsCategory>
{
    public void Configure(EntityTypeBuilder<NewsCategory> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<NewsCategory, int>();
        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.Property(x=>x.Name).IsRequired().HasMaxLength(250).IsUnicode(); ; 
        builder.Property(x => x.Slug)
        .IsRequired()
        .HasMaxLength(255);
    }
}
