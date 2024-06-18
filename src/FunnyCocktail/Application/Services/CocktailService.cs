using Application.DTOS;
using Application.Interfaces;
using Domain.Data;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Services
{
    public class CocktailService(ApplicationDbContext context, IConfiguration configuration) : ICocktailService
    {
        private static readonly string Env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        private readonly ApplicationDbContext _context = context;

        private readonly string _imageUri = configuration[$"Image:Uri:{Env}"]!;

        private readonly static string _targetPath = "images";
        private readonly static string _currentDirectory = Directory.GetCurrentDirectory();
        private readonly string _targetDirectory = Path.Combine(_currentDirectory, _targetPath);

        public async Task<bool> CreateCocktailAsync(CreateCocktailDTO create, CancellationToken cancellationToken = default)
        {
            if (await _context.Cocktails.AnyAsync(c => c.Name.ToLower() == create.Name.ToLower()))
                throw new ArgumentException("Коктейль с таким именен уже есть");

            var cocktail = new Cocktail()
            {
                Name = create.Name,
                Description = create.Description ?? "Описание отсутствует",
                AuthorId = create.AuthorId,
            };
            await _context.Cocktails.AddAsync(cocktail, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            if (create.Base64Image != null)
            {
                byte[] imagesData = Convert.FromBase64String(create.Base64Image);
                string fileName = $"{cocktail.Id}-{create.AuthorId}.jpg";

                string filePath = Path.Combine(_targetDirectory, fileName);
                await File.WriteAllBytesAsync(filePath, imagesData, cancellationToken);
                cocktail.ImageUri = $"{_imageUri}{fileName}"; 
            }


            foreach(var ingredient in create.Ingredients)
            {
                await _context.CocktailIngredients.AddAsync(new CocktailIngredient()
                {
                    CocktailId = cocktail.Id,
                    IngredientId = ingredient
                }, cancellationToken);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<Cocktail> GetAsync(Guid Id, CancellationToken cancellationToken = default) =>
            await _context.Cocktails
                .Include(c => c.Author)
                .ThenInclude(a => a!.UserAdditionalInfo)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == Id, cancellationToken)
                ?? throw new ArgumentNullException("Коктейль не найден!");

        public async Task<List<Cocktail>> GetAllCocktailsAsync(CancellationToken cancellationToken = default) =>
            await _context.Cocktails
                .AsNoTracking()
                .Include(c => c.Author)
                .ToListAsync(cancellationToken);

        public async Task<List<Cocktail>> GetAllCocktailsByIngredientIdAsync(Guid ingredientId, 
            CancellationToken cancellationToken = default) =>
            await _context.CocktailIngredients
                .AsNoTracking()
                .Where(i => i.IngredientId == ingredientId)
                .Select(b => new Cocktail()
                {
                    Id = b.Cocktail!.Id,
                    Author = b.Cocktail!.Author,
                    Name = b.Cocktail!.Name,
                    Description = b.Cocktail!.Description,
                    ImageUri = b.Cocktail!.ImageUri
                })
                .ToListAsync(cancellationToken);

        public async Task<List<Cocktail>> GetAllCocktailsByNameAsync(string name, 
            CancellationToken cancellationToken = default) =>
            await _context.Cocktails
                .AsNoTracking()
                .Where(c => c.Name.ToLower().Contains(name.ToLower()))
                .Select(b => new Cocktail()
                {
                    Id = b.Id,
                    Author = b.Author,
                    Name = b.Name,
                    Description = b.Description,
                    ImageUri = b.ImageUri
                })
                .ToListAsync(cancellationToken);
    }
}