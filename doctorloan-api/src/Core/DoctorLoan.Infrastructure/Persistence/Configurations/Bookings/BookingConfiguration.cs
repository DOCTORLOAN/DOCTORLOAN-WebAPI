using DoctorLoan.Domain.Entities.Bookings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Bookings;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<Booking, int>();

        builder.Property(x => x.Type).IsRequired();
        builder.Property(x => x.CustomerId).IsRequired();

        builder.HasOne(s => s.Customer)
          .WithOne()
          .HasForeignKey<Booking>(x => x.CustomerId)
          .OnDelete(DeleteBehavior.Restrict)
          .IsRequired(true);

        builder.HasOne(s => s.CustomerAddresses)
         .WithOne()
         .HasForeignKey<Booking>(x => x.CustomerAddressId)
         .OnDelete(DeleteBehavior.Restrict)
         .IsRequired(false);
    }
}