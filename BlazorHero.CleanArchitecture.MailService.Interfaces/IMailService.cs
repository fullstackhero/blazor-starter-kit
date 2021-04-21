using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.MailService.Interfaces.Requests;

namespace BlazorHero.CleanArchitecture.MailService.Interfaces
{
    public interface IMailService
    {
        Task SendAsync(MailRequest request);
    }
}
