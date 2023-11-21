using CleanBlazor.Contracts.Mail;

namespace CleanBlazor.Application.Abstractions.Infrastructure.Services;

public interface IMailService
{
    Task SendAsync(MailRequest request);
}
