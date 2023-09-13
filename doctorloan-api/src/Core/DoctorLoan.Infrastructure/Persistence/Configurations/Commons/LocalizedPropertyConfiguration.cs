using DoctorLoan.Domain.Entities.Commons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Commons;


public class LocalizedPropertyConfiguration : IEntityTypeConfiguration<LocalizedProperty>
{
    public void Configure(EntityTypeBuilder<LocalizedProperty> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<LocalizedProperty, int>();
        builder.Property(x => x.EntityId).IsRequired();
        builder.Property(x => x.LocaleKeyGroup).IsRequired();
        builder.Property(x => x.LocaleValue).IsRequired();
        builder.Property(x => x.LanguageId).IsRequired();
    }
}