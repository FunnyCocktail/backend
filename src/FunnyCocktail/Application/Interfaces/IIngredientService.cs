using Application.DTOS;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IIngredientService
    {
        public Task<Ingredient> GetAsync(Guid Id, CancellationToken cancellationToken = default);
        public Task<List<Ingredient>> GetAllAsync(CancellationToken cancellationToken = default);
        public Task CreateAsync(CreateCocktailDTO create, CancellationToken cancellationToken = default);
    }
}
