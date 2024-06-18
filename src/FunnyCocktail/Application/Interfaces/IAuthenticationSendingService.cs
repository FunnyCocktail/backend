namespace Application.Interfaces
{
    public interface IAuthenticationSendingService
    {
        public Task ForgotPasswordAsync(string data, CancellationToken cancellationToken = default);
    }
}
