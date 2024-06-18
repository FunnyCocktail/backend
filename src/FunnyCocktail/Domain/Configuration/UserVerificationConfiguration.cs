using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configuration
{
    public class UserVerificationConfiguration : IEntityTypeConfiguration<UserVerification>
    {
        public void Configure(EntityTypeBuilder<UserVerification> builder)
        {
            builder.ToTable(nameof(UserVerification));

            builder.HasIndex(c => c.UserId)
                .IsUnique(false);
            builder.HasIndex(c => c.Code)
                .IsUnique();
        }
    }
}
