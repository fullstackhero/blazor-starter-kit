using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Features.Dashboards.Queries.GetData;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Dashboard
{
    public interface IDashboardManager : IManager
    {
        Task<IResult<DashboardDataResponse>> GetDataAsync();
    }
}