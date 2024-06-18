using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configuration
{
    partial class CocktailConfiguration : IEntityTypeConfiguration<Cocktail>
    {
        public void Configure(EntityTypeBuilder<Cocktail> builder)
        {
            builder.ToTable(nameof(Cocktail));

            builder.HasIndex(c => c.AuthorId)
                .IsUnique(false);
            builder.HasIndex(c => c.Name)
                .IsUnique(false);

            builder.Property(c => c.Name)
                .IsRequired();
            builder.Property(c => c.Description)
                .IsRequired(false);
            builder.Property(c => c.ImageUri)
                .IsRequired(false);
        }
    }
}
