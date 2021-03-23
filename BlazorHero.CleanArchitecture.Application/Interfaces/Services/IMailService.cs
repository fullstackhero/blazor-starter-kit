using BlazorHero.CleanArchitecture.Application.Requests.Mail;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Interfaces.Services
{
    public interface IMailService
    {
        Task SendAsync(MailRequest request);
    }
}