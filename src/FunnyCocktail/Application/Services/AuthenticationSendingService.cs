using Application.Interfaces;
using Application.Models.Email;

namespace Application.Services
{
    public class AuthenticationSendingService(UserService userService, IEmailSender emailSender) : IAuthenticationSendingService
    {
        private readonly UserService _userService = userService;
        private readonly IEmailSender _emailSender = emailSender;
        public async Task ForgotPasswordAsync(string data, CancellationToken cancellationToken = default)
        {
            var user = await _userService.GetAsync(data, cancellationToken);
            int code = GenerateCode();

            await _emailSender.SendVerificationEmailAsync(new EmailTemplate()
            {
                Email = data,
                Subject = "Изменение пароля",
                Body = new EmailBodyTemplate()
                {
                    Body = "Запрос на изменение пароля!\n" +
                    $"Ваш код для подтверждения: {code}\n" +
                    $"Если это были не вы - проигнорируйте данное сообщение!"
                }
            }, code, cancellationToken);
        }
        public static int GenerateCode()
        {
            Random random = new();
            return random.Next(1, 9999);
        }
    }
}
