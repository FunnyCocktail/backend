using Application.Interfaces;
using Domain.Data;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class UserService(ApplicationDbContext context) 
    {
        public async Task<User> GetAsync(string email, CancellationToken cancellationToken = default) =>
            await context.Users.
            AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower(), cancellationToken)
            ?? throw new ArgumentException("Пользователь не найден");

        public async Task<User> GetAsync(Guid Id, CancellationToken cancellationToken = default) =>
            await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => Id == u.Id, cancellationToken)
            ?? throw new ArgumentException("Пользователь не найден");

        public string GetUserRole(User user)
        {
            var foundUser = context.Users
                .AsNoTracking()
                .Include(u => u.UserAdditionalInfo)
                .ThenInclude(u => u!.Role)
                .FirstOrDefault(u => u.Id == user.Id);
            string? roleName = foundUser?.UserAdditionalInfo!.Role!.Name;
            return roleName ?? "not found role";
        }
    }
}