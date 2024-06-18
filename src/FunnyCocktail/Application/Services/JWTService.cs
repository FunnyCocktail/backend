using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services
{
    public class JWTService(IConfiguration configuration, UserService userService) : IJWTService
    {
        private static readonly string Env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIROMENT") ?? "Development";
        private readonly double _accessTokenValidTime = double.Parse(configuration[$"JWT:AccessTokenValidityInMinutes:{Env}"]!);
        private readonly double _refreshTokenValidTime = double.Parse(configuration[$"JWT:AccessTokenValidityInMinutes:{Env}"]!);
        private readonly string? _validAudience = configuration[$"JWT:ValidAudience:{Env}"]!;
        private readonly string? _validIssuer = configuration[$"JWT:ValidIssuer:{Env}"]!;
        private readonly byte[] _secret = Encoding.ASCII.GetBytes(configuration[$"JWT:Secret:{Env}"]!);
        private readonly UserService _userService = userService;

        public TokensPair GenerateTokenPair(in User user)
        {
            var accessToken = GenerateToken(GenerateAccessTokenClaims(user), TimeSpan.FromMinutes(_accessTokenValidTime));
            var refresToken = GenerateToken(GenerateTokenClaims(in user, "refresh"), TimeSpan.FromDays(_refreshTokenValidTime));
            return new TokensPair()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                RefreshToken = new JwtSecurityTokenHandler().WriteToken(refresToken),
                AccessExp = accessToken.ValidTo,
                RefreshExp = refresToken.ValidTo
            };
        }

        private JwtSecurityToken GenerateToken(Claim[] claims, TimeSpan timeSpan)
        {
            var credentials = new SigningCredentials(new SymmetricSecurityKey(_secret), SecurityAlgorithms.HmacSha256);

            return claims == null ?
                throw new ArgumentNullException(nameof(claims))
                : new JwtSecurityToken(_validIssuer, _validAudience, claims,
                expires: DateTime.UtcNow.Add(timeSpan),
                signingCredentials: credentials);
        }

        private Claim[] GenerateAccessTokenClaims(in User user) =>
            [
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, _userService.GetUserRole(user)),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
            ];

        private Claim[] GenerateTokenClaims(in User user, string type) =>
            [
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("TokenType", type)
            ];


        public ClaimsPrincipal GetUserClaims(string token)
        {
            var secret = Encoding.ASCII.GetBytes(configuration[$"JWT:Secret:{Env}"]!);
            var parametrs = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secret),
                ValidateLifetime = false,
            };

            var handler = new JwtSecurityTokenHandler();

            try
            {
                var claims = handler.ValidateToken(token, parametrs, out var securityToken);

                if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                    !jwtSecurityToken.Header.Alg.Equals("HS256", StringComparison.OrdinalIgnoreCase))
                    throw new SecurityTokenException();

                return claims;
            }
            catch (SecurityTokenException)
            {
                throw new UnauthorizedAccessException("Token not valid");
            }
        }
    }
}
