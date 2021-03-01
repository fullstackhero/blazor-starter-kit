using System.Collections.Generic;

namespace BlazorHero.CleanArchitecture.Client.ViewModels
{
    public class UsersViewModel
    {
        public IEnumerable<UserViewModel> Users = new List<UserViewModel>();
    }
}