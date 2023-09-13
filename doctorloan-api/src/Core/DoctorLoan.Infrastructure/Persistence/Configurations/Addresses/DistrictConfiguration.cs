using DoctorLoan.Domain.Entities.Addresses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Orders;

public class DistrictConfiguration : IEntityTypeConfiguration<District>
{
    public void Configure(EntityTypeBuilder<District> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<District, int>();

        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Code).IsRequired();
        builder.HasIndex(x => x.Code).IsUnique();
        builder.Property(x => x.Type).IsRequired();
        builder.Property(x => x.ProvinceId).IsRequired();
        builder.Property(x => x.SortBy).IsRequired();
        builder.Property(x => x.IsPublished).IsRequired();

        builder.HasOne(x => x.Province)
        .WithMany(x => x.Districts)
        .HasForeignKey(x => x.ProvinceId)
        .OnDelete(DeleteBehavior.Cascade)
        .IsRequired(false);
    }
}