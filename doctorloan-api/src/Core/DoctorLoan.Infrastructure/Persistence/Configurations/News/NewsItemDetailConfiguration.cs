using DoctorLoan.Domain.Entities.News;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Products;

public class NewsItemDetailConfiguration : IEntityTypeConfiguration<NewsItemDetail>
{
    public void Configure(EntityTypeBuilder<NewsItemDetail> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<NewsItemDetail, int>();      
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(500)
            .IsUnicode();
        builder.Property(x => x.Short)
       .HasMaxLength(700)
       .IsUnicode();

        builder.HasOne(x => x.NewsItem)
            .WithMany(x => x.NewsItemDetails)
            .HasForeignKey(x => x.NewsId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
      
    }
}
