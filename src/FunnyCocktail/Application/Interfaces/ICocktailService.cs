using Application.DTOS;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface ICocktailService
    {
        public Task<bool> CreateCocktailAsync(CreateCocktailDTO create, CancellationToken cancellationToken = default);  
        public Task<Cocktail> GetAsync(Guid Id, CancellationToken cancellationToken = default);
        public Task<List<Cocktail>> GetAllCocktailsAsync(CancellationToken cancellationToken = default);
        public Task<List<Cocktail>> GetAllCocktailsByIngredientIdAsync(Guid ingredientId, CancellationToken cancellationToken = default);
        public Task<List<Cocktail>> GetAllCocktailsByNameAsync(string name, CancellationToken cancellationToken = default);
    }
}
