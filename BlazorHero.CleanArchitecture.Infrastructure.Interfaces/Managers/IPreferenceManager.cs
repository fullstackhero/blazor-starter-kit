using BlazorHero.CleanArchitecture.Infrastructure.Interfaces.Settings;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Infrastructure.Interfaces.Managers
{
    public interface IPreferenceManager
    {
        Task SetPreference(IPreference preference);

        Task<IPreference> GetPreference();
    }
}
