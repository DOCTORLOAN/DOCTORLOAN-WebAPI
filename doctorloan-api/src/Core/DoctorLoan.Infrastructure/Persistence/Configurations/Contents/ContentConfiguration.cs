using DoctorLoan.Domain.Entities.Contents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Contents;

public class ContentConfiguration : IEntityTypeConfiguration<Content>
{
    public void Configure(EntityTypeBuilder<Content> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<Content, int>();

        builder.HasIndex(x => x.Code).IsUnique().HasFilter(@"""Status"" = 10");
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Status).IsRequired();

        builder.HasOne(s => s.Media)
                .WithOne()
                .HasForeignKey<Content>(s => s.MediaId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
    }
}