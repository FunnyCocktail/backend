using Application.Interfaces;
using Application.Models;
using Domain.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class AuthenticationConfirmService(ApplicationDbContext context, 
        UserService userService) : IAuthenticationConfirmService
    {
        private readonly UserService _userService = userService;
        public async Task<bool> ConfirmEmailAsync(Verification verification, CancellationToken cancellationToken = default)
        {
            if (!await context.UserVerifications.AnyAsync(u => u.UserId == verification.UserId
                && u.Code == verification.Code))
                throw new ArgumentException("Verification Error");

            var foundUser = await _userService.GetAsync(verification.UserId, cancellationToken);
            var foundUserAdditional = await context.UserAdditionalInfos
                .FirstOrDefaultAsync(u => verification.UserId == u.UserId, cancellationToken)
                ?? throw new ArgumentException("Пользователь не найден!");
            var foundUserVerification = await context.UserVerifications
                .AsNoTracking()
                .OrderByDescending(u => u.DateTimeVerification)
                .FirstOrDefaultAsync(u => u.UserId == verification.UserId
                && u.Status == false, cancellationToken)
                ?? throw new ArgumentException("Пользователь не найден!");
            foundUserAdditional.IsVerify = true;
            foundUserVerification.Status = true;
            foundUserVerification.DateTimeVerification = DateTime.UtcNow;
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> UpdatePasswordAsync(UpdateData data,
            CancellationToken cancellationToken = default)
        {
            var user = await _userService.GetAsync(data.Email, cancellationToken);
            AuthenticationService.UpdateUserPassword(ref user, data.NewData);
            if (!await context.UserVerifications.AnyAsync(u => u.UserId == user.Id
                && u.Code == data.Code, cancellationToken))
                throw new ArgumentException("Проверьте правильность введённых данных");
            var foundUserVerification = await context.UserVerifications
                .AsNoTracking()
                .OrderByDescending(u => u.DateTimeVerification)
                .FirstOrDefaultAsync(u => u.UserId == user.Id
                && u.Status == false, cancellationToken)
                ?? throw new ArgumentException("Пользователь не найден!");
            try
            {
                foundUserVerification.Status = true;
                context.Users.Update(user);
                await context.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}
