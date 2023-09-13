using DoctorLoan.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Orders;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<Order, int>();

        builder.HasIndex(x => x.OrderNo).IsUnique();
        builder.Property(x => x.OrderNo).IsRequired();
        builder.Property(x => x.CustomerId).IsRequired();
        builder.Property(x => x.Status).IsRequired();

        builder.HasMany(x => x.OrderItems)
        .WithOne(s => s.Order)
        .HasForeignKey(x => x.OrderId)
        .OnDelete(DeleteBehavior.Restrict)
        .IsRequired(false);
    }
}
