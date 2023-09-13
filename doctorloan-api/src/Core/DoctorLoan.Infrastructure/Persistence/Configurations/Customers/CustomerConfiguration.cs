using DoctorLoan.Domain.Entities.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Customers;
public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<Customer, int>();

        builder.HasIndex(x => x.UID).IsUnique();
        builder.Property(x => x.UID).IsRequired();
        builder.Property(x => x.FullName).IsRequired();
        builder.Property(x => x.Gender).IsRequired();
    }
}