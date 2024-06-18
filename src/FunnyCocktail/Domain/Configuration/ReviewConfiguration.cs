using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configuration
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasIndex(r => r.UserId)
                .IsUnique(false);
            builder.HasIndex(r => r.CocktailId)
                .IsUnique(false);

            builder.Property(r => r.Value)
                .IsRequired();
        }
    }
}
