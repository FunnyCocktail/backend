using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Domain.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<UserAdditionalInfo> UserAdditionalInfos => Set<UserAdditionalInfo>();
        public DbSet<UserVerification> UserVerifications => Set<UserVerification>();
        public DbSet<Ingredient> Ingredients => Set<Ingredient>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Cocktail> Cocktails => Set<Cocktail>();
        public DbSet<CocktailGrade> CocktailGrades => Set<CocktailGrade>();
        public DbSet<CocktailIngredient> CocktailIngredients => Set<CocktailIngredient>();
        public DbSet<Review> Reviews => Set<Review>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
