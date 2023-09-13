using DoctorLoan.Domain.Entities.News;
using DoctorLoan.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Products;

public class NewsMediaConfiguration : IEntityTypeConfiguration<NewsMedia>
{
    public void Configure(EntityTypeBuilder<NewsMedia> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<NewsMedia, int>();
        builder.HasOne(x=>x.NewsItem)
            .WithMany(x=>x.NewsMedias)
            .HasForeignKey(x=>x.NewsId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Media)
        .WithMany()
        .HasForeignKey(x => x.MediaId)
        .IsRequired()
        .OnDelete(DeleteBehavior.Cascade);
    }
}
