using DoctorLoan.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Products;

public class EntityLanguageConfiguration : IEntityTypeConfiguration<EntityLanguage>
{
    public void Configure(EntityTypeBuilder<EntityLanguage> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<EntityLanguage, int>();
        builder.Property(x => x.PropertyName)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(x => x.EntityId)
           .IsRequired();
        builder.Property(x => x.EntityName)
           .IsRequired()
           .HasMaxLength(200);
        builder.Property(x => x.Value)
        .IsRequired();

    }
}
