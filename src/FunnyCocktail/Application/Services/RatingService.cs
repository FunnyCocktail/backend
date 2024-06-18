using Application.DTOS;
using Application.Interfaces;
using Domain.Data;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class RatingService(ApplicationDbContext context) : IRatingService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<List<CocktailGrade>> GetAllGradesAsync(Guid Id, CancellationToken cancellationToken = default) =>
            await _context.CocktailGrades
                .AsNoTracking()
                .Where(cg => cg.CocktailId == Id)
                .Include(cg => cg.User)
                .ThenInclude(u => u!.UserAdditionalInfo)
                .ToListAsync(cancellationToken);

        public async Task<bool> RateCocktailAsync(RateCocktailDTO rate, CancellationToken cancellationToken = default)
        {
            if (await _context.CocktailGrades.AnyAsync(cg => cg.CocktailId == rate.CocktailId
                && cg.UserId == rate.UserId, cancellationToken))
                throw new ArgumentException("Вы уже поставили оценку коктейлю!");
            await _context.CocktailGrades.AddAsync(new CocktailGrade()
            {
                CocktailId = rate.CocktailId,
                UserId = rate.UserId,
                Grade = rate.Grade
            }, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

    }
}
