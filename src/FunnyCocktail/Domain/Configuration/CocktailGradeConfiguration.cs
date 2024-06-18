using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configuration
{
    public class CocktailGradeConfiguration : IEntityTypeConfiguration<CocktailGrade>
    {
        public void Configure(EntityTypeBuilder<CocktailGrade> builder)
        {
            builder.ToTable(nameof(CocktailGrade));

            builder.HasIndex(c => c.UserId)
                .IsUnique(false);
            builder.HasIndex(c => c.CocktailId)
                .IsUnique(false);

            builder.Property(c => c.Grade)
                .IsRequired();
        }
    }
}
