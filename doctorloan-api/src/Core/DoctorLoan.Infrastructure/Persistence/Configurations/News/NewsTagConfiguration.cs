using DoctorLoan.Domain.Entities.News;
using DoctorLoan.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Products;

public class NewsTagConfiguration : IEntityTypeConfiguration<NewsTag>
{
    public void Configure(EntityTypeBuilder<NewsTag> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<NewsTag, int>();
     
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200)
            .IsUnicode();
     
    }
}
