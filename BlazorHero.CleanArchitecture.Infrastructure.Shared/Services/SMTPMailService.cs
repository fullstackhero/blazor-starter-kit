using BlazorHero.CleanArchitecture.Application.Configurations;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using BlazorHero.CleanArchitecture.Application.Requests.Mail;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Infrastructure.Shared.Services
{
    public class SMTPMailService : IMailService
    {
        public MailConfiguration _config { get; }
        public ILogger<SMTPMailService> _logger { get; }

        public SMTPMailService(IOptions<MailConfiguration> config, ILogger<SMTPMailService> logger)
        {
            _config = config.Value;
            _logger = logger;
        }

        public async Task SendAsync(MailRequest request)
        {
            try
            {
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(request.From ?? _config.From);
                email.To.Add(MailboxAddress.Parse(request.To));
                email.Subject = request.Subject;
                var builder = new BodyBuilder();
                builder.HtmlBody = request.Body;
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect(_config.Host, _config.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_config.UserName, _config.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }
    }
}