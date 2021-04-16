using BlazorHero.CleanArchitecture.Shared.Settings;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Shared.Managers
{
    public interface IPreferenceManager
    {
        Task SetPreference(IPreference preference);

        Task<IPreference> GetPreference();
    }
}
