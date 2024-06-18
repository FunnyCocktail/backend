using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configuration
{
    public class CocktailIngredientConfiguration : IEntityTypeConfiguration<CocktailIngredient>
    {
        public void Configure(EntityTypeBuilder<CocktailIngredient> builder)
        {
            builder.ToTable(nameof(CocktailIngredient));

            builder.HasIndex(c => c.CocktailId)
                .IsUnique(false);
            builder.HasIndex(c => c.IngredientId)
                .IsUnique(false);
        }
    }
}
