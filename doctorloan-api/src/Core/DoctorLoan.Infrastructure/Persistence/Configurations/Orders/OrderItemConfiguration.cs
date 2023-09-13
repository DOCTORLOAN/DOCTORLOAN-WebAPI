using DoctorLoan.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Orders;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<OrderItem, int>();

        builder.Property(x => x.OrderId).IsRequired();
        builder.Property(x => x.ProductItemId).IsRequired();


        builder.HasOne(x => x.ProductItem)
        .WithMany(s => s.OrderItems)
        .HasForeignKey(x => x.ProductItemId)
        .OnDelete(DeleteBehavior.Restrict)
        .IsRequired(false);
    }
}