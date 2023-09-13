using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DoctorLoan.Domain.Entities.Addresses;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Orders;

public class WardConfiguration : IEntityTypeConfiguration<Ward>
{
    public void Configure(EntityTypeBuilder<Ward> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<Ward, int>();

        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Code).IsRequired();
        builder.Property(x => x.Type).IsRequired();
        builder.Property(x => x.Longitude).IsRequired();
        builder.Property(x => x.Latitude).IsRequired();
        builder.Property(x => x.DistrictId).IsRequired();
        builder.Property(x => x.SortBy).IsRequired();
        builder.Property(x => x.IsPublished).IsRequired();

        builder.HasOne(x => x.District)
        .WithMany(x => x.Wards)
        .HasForeignKey(x => x.DistrictId)
        .OnDelete(DeleteBehavior.Cascade)
        .IsRequired(false);
    }
}