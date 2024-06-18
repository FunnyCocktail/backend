using Application.Interfaces;
using Application.Models;
using Application.Models.Email;
using Domain.Data;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Application.Services
{
    public class AuthenticationService(ApplicationDbContext context, 
        IJWTService jwtService, IEmailSender emailSender) : IAuthenticationService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IJWTService _jwtService = jwtService;
        private readonly IEmailSender _emailSender = emailSender;

        public async Task<TokensPair> SignInAsync(UserRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username.ToLower() == request.Username.ToLower()
                || u.Email.ToLower() == request.Email.ToLower())
                ?? throw new ArgumentException("Пользователь не найден");
            if (!ValidationService.CheckValidPassword(in user, request.Password))
                throw new ArgumentException("Пароли не совпадают");

            return _jwtService.GenerateTokenPair(user);
        }

        public async Task SignUpAsync(UserRequest request, CancellationToken cancellationToken = default)
        {
            if (!ValidationService.CheckCorrectLogin(request.Username))
                throw new BadHttpRequestException("Некорректный логин");
            if (!ValidationService.CheckCorrectEmail(request.Email))
                throw new BadHttpRequestException("Некорректная почта");
            if (await _context.Users.AnyAsync(u => u.Email.ToLower() == request.Email.ToLower()
                || u.Username.ToLower() == request.Username.ToLower(), cancellationToken))
                throw new BadHttpRequestException("Пользователь существует");

            var user = new User()
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = HashService.GetHash(request.Password)
            };

            var role = await _context.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Name == "user", cancellationToken);

            int code = GenerateCode();

            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await _emailSender.SendVerificationEmailAsync(new EmailTemplate
            {
                Email = request.Email,
                Subject = "Подтверждение аккаунта",
                Body = new EmailBodyTemplate
                {
                    Body = $"Здравствуйте, {request.Username}\n" +
                    $"Добро пожаловать в FunnyCocktail!\n" +
                    $"Ваш код для подтверждения аккаунта: {code}"
                }
            }, code, cancellationToken);
            await _context.UserAdditionalInfos.AddAsync(new UserAdditionalInfo()
            {
                ImageUri = null,
                RoleId = role!.Id,
                IsVerify = false,
                SecretKey = GenerateSecretKey(ref user),
                UserId = user.Id
            }, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public static string GenerateSecretKey(ref User user) =>
            HashService.GetHash(user.Username[2].ToString());

        public async Task<UserResponse> GetUserFromTokenAsync(string token, string type, CancellationToken cancellationToken = default)
        {
            var claims = _jwtService.GetUserClaims(token);

            var id = claims.Claims.Single(c => 
                c.Type == ClaimTypes.NameIdentifier).Value;

            var user = await _context.Users
                .Include(u => u.UserAdditionalInfo)
                .ThenInclude(u => u!.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => Guid.Parse(id) == u.Id, cancellationToken)
                ?? throw new ArgumentException("Пользователь не найден");

            if (!ValidationService.IsValidToken(user, claims, type))
                throw new ArgumentException($"Не валидный {type} токен");

            return new UserResponse()
            {
                Id = user.Id,
                UserRole = user.UserAdditionalInfo!.Role!.Name,
                ImageUri = user.UserAdditionalInfo.ImageUri
            };
        }

        public static void UpdateUserPassword(ref User user, string password)
        {
            ValidationService.CheckCorrectPassword(password);

            user.PasswordHash = HashService.GetHash(password);
        }

        public static int GenerateCode()
        {
            Random random = new();
            return random.Next(1, 9999);
        }
    }
}
