using DoctorLoan.Domain.Entities.Emails;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Emails;



public class EmailRequestConfiguration : IEntityTypeConfiguration<EmailRequest>
{
    public void Configure(EntityTypeBuilder<EmailRequest> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<EmailRequest, int>();

        builder.Property(x => x.Type).IsRequired();
        builder.Property(x => x.Email).IsRequired();
    }
}
