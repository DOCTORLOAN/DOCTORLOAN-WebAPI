using DoctorLoan.Domain.Entities.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Customers;

public class CustomerAddressConfiguration : IEntityTypeConfiguration<CustomerAddress>
{
    public void Configure(EntityTypeBuilder<CustomerAddress> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<CustomerAddress, int>();

        builder.Property(x => x.CustomerId).IsRequired();
        builder.Property(x => x.AddressId).IsRequired();
        builder.HasIndex(x => new { x.CustomerId, x.AddressId }).IsUnique();

        builder.HasOne(x => x.Customer)
        .WithMany(x => x.CustomerAddresses)
        .HasForeignKey(x => x.CustomerId)
        .OnDelete(DeleteBehavior.Restrict)
        .IsRequired(false);

        builder.HasOne(x => x.Address)
        .WithMany(s => s.CustomerAddresses)
        .HasForeignKey(x => x.AddressId)
        .OnDelete(DeleteBehavior.Restrict)
        .IsRequired(false);
    }
}
