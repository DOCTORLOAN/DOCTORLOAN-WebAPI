using DoctorLoan.Domain.Entities.Banks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Banks;

public class BankBranchConfiguration : IEntityTypeConfiguration<BankBranch>
{
    public void Configure(EntityTypeBuilder<BankBranch> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<BankBranch, int>();

        builder.Property(x => x.Code).IsRequired();
        builder.HasIndex(x => new { x.BankId, x.BranchId }).IsUnique();
        builder.HasQueryFilter(x => !x.IsDelete);

        builder.HasOne(x => x.Bank)
             .WithMany(x => x.BankBranchs)
             .HasForeignKey(x => x.BankId)
             .IsRequired()
             .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Branch)
            .WithMany(x => x.BankBranchs)
            .HasForeignKey(x => x.BranchId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
