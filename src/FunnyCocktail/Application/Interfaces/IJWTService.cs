using Application.Models;
using Domain.Entities;
using System.Security.Claims;

namespace Application.Interfaces
{
    public interface IJWTService
    {
        public ClaimsPrincipal GetUserClaims(string token);
        public TokensPair GenerateTokenPair(in User user);
    }
}
