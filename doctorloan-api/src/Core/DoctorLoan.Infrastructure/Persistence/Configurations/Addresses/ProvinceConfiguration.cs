using DoctorLoan.Domain.Entities.Addresses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Orders;

public class ProvinceConfiguration : IEntityTypeConfiguration<Province>
{
    public void Configure(EntityTypeBuilder<Province> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<Province, int>();

        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Code).IsRequired();
        builder.Property(x => x.Type).IsRequired();
        builder.Property(x => x.PhoneCode).IsRequired();
        builder.Property(x => x.ZipCode).IsRequired();
        builder.Property(x => x.CountryId).IsRequired();
        builder.Property(x => x.RegionType).IsRequired();
        builder.Property(x => x.SortBy).IsRequired();
        builder.Property(x => x.IsPublished).IsRequired();

        builder.HasOne(x => x.Country)
        .WithMany(x => x.Provinces)
        .HasForeignKey(x => x.CountryId)
        .OnDelete(DeleteBehavior.Cascade)
        .IsRequired(false);
    }
}