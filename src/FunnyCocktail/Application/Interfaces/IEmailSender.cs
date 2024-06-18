using Application.Models.Email;

namespace Application.Interfaces
{
    public interface IEmailSender
    {
        public Task<bool> SendVerificationEmailAsync(EmailTemplate template, int code, CancellationToken cancellationToken = default);
    }
}
