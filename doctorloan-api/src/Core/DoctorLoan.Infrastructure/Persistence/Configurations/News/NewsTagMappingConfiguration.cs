using DoctorLoan.Domain.Entities.News;
using DoctorLoan.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Products;

public class NewsTagMappingConfiguration : IEntityTypeConfiguration<NewsTagsMapping>
{
    public void Configure(EntityTypeBuilder<NewsTagsMapping> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<NewsTagsMapping, int>();

        builder.HasOne(x => x.NewsTag)
            .WithMany()
            .HasForeignKey(x => x.NewsTagId)
            .IsRequired();
        builder.HasOne(x => x.NewsItem)
          .WithMany(x=>x.NewsTags)
          .HasForeignKey(x => x.NewsItemId)
          .IsRequired()
          .OnDelete(DeleteBehavior.Cascade);
          ;
    }
}
