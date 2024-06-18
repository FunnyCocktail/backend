using Application.DTOS;
using Application.Interfaces;
using Domain.Data;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Services
{
    public class IngredientService(ApplicationDbContext context, IConfiguration configuration) : IIngredientService
    {
        private static readonly string Env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        private readonly ApplicationDbContext _context = context;

        private readonly string _imageUri = configuration[$"Image:Uri:{Env}"]!;
        private readonly static string _targetPath = "images";
        private readonly static string _currentDirectory = Directory.GetCurrentDirectory();
        private readonly string _targetDirectory = Path.Combine(_currentDirectory, _targetPath);

        public async Task CreateAsync(CreateCocktailDTO create, CancellationToken cancellationToken = default)
        {
            if (await _context.Ingredients.AnyAsync(i => i.Name.ToLower().Contains(
                create.Name.ToLower()), cancellationToken))
                throw new ArgumentException("Такой ингредиент уже есть");

            var ingredient = new Ingredient()
            {
                Name = create.Name,
                AuthorId = create.AuthorId,
            };
            await _context.Ingredients.AddAsync(ingredient, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            if (create.Base64Image != null)
            {
                byte[] imagesData = Convert.FromBase64String(create.Base64Image);
                string fileName = $"{ingredient.Id}-{create.AuthorId}.jpg";

                string filePath = Path.Combine(_targetDirectory, fileName);
                await File.WriteAllBytesAsync(filePath, imagesData, cancellationToken);
                ingredient.ImageUri = $"{_imageUri}{fileName}";
            }
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<Ingredient>> GetAllAsync(CancellationToken cancellationToken = default) =>
            await _context.Ingredients
                .AsNoTracking()
                .Include(i => i.Author)
                .ToListAsync(cancellationToken)
                ?? throw new ArgumentNullException("Ингредиентов нет!");

        public async Task<Ingredient> GetAsync(Guid Id, CancellationToken cancellationToken = default) =>
            await _context.Ingredients
                .AsNoTracking()
                .Include(i => i.Author)
                .FirstOrDefaultAsync(i => i.Id == Id, cancellationToken)
                ?? throw new ArgumentNullException("Ингредиент не найден!");
    }
}
