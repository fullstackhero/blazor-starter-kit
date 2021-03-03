using BlazorHero.CleanArchitecture.Client.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Interfaces
{
    public interface IAdminService
    {
        Task<List<UserViewModel>> GetUsersAsync();

        
    }
}