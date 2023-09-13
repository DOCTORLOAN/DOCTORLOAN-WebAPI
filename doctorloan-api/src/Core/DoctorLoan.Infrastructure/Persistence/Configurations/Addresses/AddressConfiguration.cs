using DoctorLoan.Domain.Entities.Addresses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Addresses;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<Address, int>();


        builder.Property(x => x.AddressLine).IsRequired();
        builder.Property(x => x.CountryId).IsRequired();

        builder.HasOne(x => x.Country)
        .WithMany(x => x.Addresses)
        .HasForeignKey(x => x.CountryId)
        .OnDelete(DeleteBehavior.Restrict)
        .IsRequired(false);

        builder.HasOne(x => x.Province)
       .WithMany(x => x.Addresses)
       .HasForeignKey(x => x.ProvinceId)
       .OnDelete(DeleteBehavior.Restrict)
       .IsRequired(false);

        builder.HasOne(x => x.District)
        .WithMany(x => x.Addresses)
        .HasForeignKey(x => x.DistrictId)
        .OnDelete(DeleteBehavior.Restrict)
        .IsRequired(false);

        builder.HasOne(x => x.Ward)
        .WithMany(x => x.Addresses)
        .HasForeignKey(x => x.WardId)
        .OnDelete(DeleteBehavior.Restrict)
        .IsRequired(false);
    }
}