using Application.Models;

namespace Application.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<TokensPair> SignInAsync(UserRequest request, CancellationToken cancellationToken = default);
        public Task SignUpAsync(UserRequest request, CancellationToken cancellationToken = default);
        public Task<UserResponse> GetUserFromTokenAsync(string token, string type, CancellationToken cancellationToken = default);
    }
}
