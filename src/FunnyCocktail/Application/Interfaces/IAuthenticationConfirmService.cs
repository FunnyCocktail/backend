using Application.Models;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IAuthenticationConfirmService
    {
        public Task<bool> ConfirmEmailAsync(Verification verification, CancellationToken cancellationToken = default);
        public Task<bool> UpdatePasswordAsync(UpdateData data, CancellationToken cancellationToken = default);
    }
}
