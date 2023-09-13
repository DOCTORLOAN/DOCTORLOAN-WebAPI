using DoctorLoan.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorLoan.Infrastructure.Persistence.Configurations.Users;

public class UserBankBranchConfiguration : IEntityTypeConfiguration<UserBankBranch>
{
    public void Configure(EntityTypeBuilder<UserBankBranch> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ConfigurateBaseAudit<UserBankBranch, int>();
        builder.HasQueryFilter(x => !x.BankBranch.IsDelete);

        builder.HasOne(x => x.User)
             .WithMany(x => x.UserBankBranchs)
             .HasForeignKey(x => x.UserId)
             .IsRequired()
             .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.BankBranch)
            .WithMany(x => x.UserBankBranchs)
            .HasForeignKey(x => x.BankBranchId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
