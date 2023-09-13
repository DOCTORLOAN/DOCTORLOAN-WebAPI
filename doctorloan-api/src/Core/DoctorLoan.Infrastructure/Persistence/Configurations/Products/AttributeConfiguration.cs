using DoctorLoan.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Products;

public class AttributeConfiguration : IEntityTypeConfiguration<Domain.Entities.Products.Attribute>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Products.Attribute> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<Domain.Entities.Products.Attribute, int>();
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(70)
            .IsUnicode();
        builder.HasOne(x => x.AttributeGroup)
            .WithMany(x => x.Attributes)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

    }
}
