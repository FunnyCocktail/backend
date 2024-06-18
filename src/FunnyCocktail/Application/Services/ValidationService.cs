using Domain.Entities;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace Application.Services
{
    public class ValidationService
    {
        public static bool CheckCorrectEmail(string email)
        {
            if (!MailAddress.TryCreate(email, out var address)) return false;
            var hostParts = address.Host.Split('.');
            if (hostParts.Length == 1) return false;
            if (hostParts.Any(p => p == string.Empty)) return false;
            if (hostParts[^1].Length < 2) return false;

            return !address.User.Contains(' ')
                && address.User.Split('.').All(p => p != string.Empty);
        }

        public static void CheckCorrectPassword(string password)
        {
            if (password.Length <= 3 || password.Length >= 50)
                throw new ArgumentException("Пароль должен быть от 5 до 50 символов");
        }

        public static bool CheckCorrectLogin(string login) => login != null 
            && login.Length >= 3;

        public static bool CheckValidPassword(in User user, string password) =>
            HashService.GetHash(password) == user.PasswordHash;

        public static bool IsValidToken(in User user, ClaimsPrincipal claimsPrincipal, string type) =>
            DateTime.UtcNow < DateTimeOffset.FromUnixTimeSeconds(long.Parse(
                claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "exp")?.Value ?? "0")
                ).UtcDateTime &&
            user.Email == claimsPrincipal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
            && type == claimsPrincipal?.Claims.FirstOrDefault(c => c.Type == "TokenType")?.Value;
    }
}
