using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using Domain.Data;
using Application.Models.Email;

namespace Application.Services
{
    public class EmailSender(IConfiguration configuration, 
        ApplicationDbContext context, UserService userService) : IEmailSender
    {
        private static readonly string Env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        private readonly ApplicationDbContext _context = context;
        private readonly UserService _userService = userService;

        private readonly string _host = configuration[$"EmailConfig:Host:{Env}"]!;
        private readonly int _port = int.Parse(configuration[$"EmailConfig:Port:{Env}"]!);
        private readonly string _email = configuration[$"EmailConfig:Email:{Env}"]!;
        private readonly string _password = configuration[$"EmailConfig:Password:{Env}"]!;

        public async Task<bool> SendVerificationEmailAsync(EmailTemplate template, int code,
            CancellationToken cancellationToken = default)
        {
            var msg = new MimeMessage();

            msg.From.Add(new MailboxAddress("FunnyCocktail", _email));
            msg.To.Add(new MailboxAddress(template.Email, template.Email));
            msg.Subject = template.Subject;
            msg.Body = new TextPart() { Text = template.Body.Body };

            using var client = new SmtpClient();
            await client.ConnectAsync(_host, _port, true, cancellationToken);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            await client.AuthenticateAsync(_email, _password, cancellationToken);

            var user = await _userService.GetAsync(template.Email, cancellationToken);

            try
            {
                await client.SendAsync(msg, cancellationToken);
                await _context.UserVerifications.AddAsync(new UserVerification()
                {
                    UserId = user.Id,
                    Code = code,
                    Status = false,
                    DateTimeVerification = DateTime.UtcNow
                }, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (SmtpCommandException) { return false; }
            catch (Exception) { return false; }
        }
    }
}
