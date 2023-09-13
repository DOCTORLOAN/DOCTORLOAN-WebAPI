using DoctorLoan.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Users;

public class UserMediaConfiguration : IEntityTypeConfiguration<UserMedia>
{
    public void Configure(EntityTypeBuilder<UserMedia> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<UserMedia, int>();

        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.MediaId).IsRequired();
        builder.Property(x => x.Type).IsRequired();

        builder.HasOne(s => s.Media)
         .WithOne()
         .HasForeignKey<UserMedia>(a => a.MediaId)
         .OnDelete(DeleteBehavior.Restrict);
    }
}