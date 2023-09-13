using DoctorLoan.Domain.Entities.Medias;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Configurations;

public class MediaConfiguration : IEntityTypeConfiguration<Media>
{
    public void Configure(EntityTypeBuilder<Media> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<Media, long>();

        builder.Property(x => x.Path).IsRequired();
        builder.Property(x => x.Type).IsRequired();
        builder.Property(x => x.OriginalName).IsRequired();
        builder.Property(x => x.Status).IsRequired();
    }
}