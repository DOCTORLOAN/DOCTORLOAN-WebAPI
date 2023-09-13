using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations;

public static class ConfigurationExtension
{
    public static void ConfigurateBaseAudit<T, TKey>(this EntityTypeBuilder<T> builder, bool enableSoftDelete = true) where T : AuditableEntity
    {
        builder.Property(x => x.Created).HasDefaultValueSql("now()");
        builder.Property(x => x.LastModified).HasDefaultValueSql("now()");
    }
}
