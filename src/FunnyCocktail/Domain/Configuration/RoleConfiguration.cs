using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        private readonly List<Role> _roles = [
            new Role() { Name = "user" }, new Role() { Name = "admin" }, new Role() { Name = "owner" }
            ];
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable(nameof(Role));

            foreach (var role in _roles) builder.HasData(role);
        }
    }
}
