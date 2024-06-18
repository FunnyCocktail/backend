using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configuration
{
    public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
    {
        public void Configure(EntityTypeBuilder<Ingredient> builder)
        {
            builder.ToTable(nameof(Ingredient));

            builder.HasIndex(i => i.AuthorId)
                .IsUnique(false);
            builder.HasIndex(i => i.Name)
                .IsUnique();

            builder.Property(i => i.ImageUri)
                .IsRequired(false);
            builder.Property(i => i.Name)
                .IsRequired();
        }
    }
}
