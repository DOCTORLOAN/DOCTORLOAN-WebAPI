using DoctorLoan.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Orders;

public class UserAddressConfiguration : IEntityTypeConfiguration<UserAddress>
{
    public void Configure(EntityTypeBuilder<UserAddress> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<UserAddress, int>();
        builder.HasQueryFilter(x => !x.IsDelete);

        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.AddressId).IsRequired();

        builder.HasOne(x => x.User)
        .WithMany(x => x.UserAddresses)
        .HasForeignKey(x => x.UserId)
        .OnDelete(DeleteBehavior.Restrict)
        .IsRequired(false);

        builder.HasOne(x => x.Address)
        .WithMany(s => s.UserAddresses)
        .HasForeignKey(x => x.AddressId)
        .OnDelete(DeleteBehavior.Restrict)
        .IsRequired(false);
    }
}