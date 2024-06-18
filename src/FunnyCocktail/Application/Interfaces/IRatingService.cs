using Application.DTOS;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IRatingService
    {
        public Task<bool> RateCocktailAsync(RateCocktailDTO rate, CancellationToken cancellationToken = default);
        public Task<List<CocktailGrade>> GetAllGradesAsync(Guid Id, CancellationToken cancellationToken = default);
    }
}
