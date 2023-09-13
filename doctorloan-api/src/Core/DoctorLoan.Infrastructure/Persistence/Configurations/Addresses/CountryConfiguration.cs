using DoctorLoan.Domain.Entities.Addresses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Addresses;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<Country, int>();

        builder.Property(x => x.Code).IsRequired();
        builder.HasIndex(x => x.Code).IsUnique();
        builder.Property(x => x.Code3).IsRequired();
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Capital).IsRequired();
        builder.Property(x => x.CurrencyCode).IsRequired();
        builder.Property(x => x.CurrencyName).IsRequired();
        builder.Property(x => x.PhoneCode).IsRequired();
        builder.Property(x => x.CountryNo).IsRequired();
        builder.Property(x => x.InternetCountryCode).IsRequired();
        builder.Property(x => x.Flags).IsRequired();
        builder.Property(x => x.SortBy).IsRequired();
        builder.Property(x => x.IsPublished).IsRequired();

        builder.HasMany(x => x.Provinces)
        .WithOne(x => x.Country)
        .HasForeignKey(x => x.CountryId)
        .OnDelete(DeleteBehavior.Cascade)
        .IsRequired(false);
    }
}