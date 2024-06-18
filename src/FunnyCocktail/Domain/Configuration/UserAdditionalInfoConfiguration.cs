using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configuration
{
    public class UserAdditionalInfoConfiguration : IEntityTypeConfiguration<UserAdditionalInfo>
    {
        public void Configure(EntityTypeBuilder<UserAdditionalInfo> builder)
        {
            builder.ToTable(nameof(UserAdditionalInfo));

            builder.HasIndex(u => u.UserId)
                .IsUnique();
            builder.HasIndex(u => u.RoleId)
                .IsUnique(false);

            builder.HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleId);
        }
    }
}
