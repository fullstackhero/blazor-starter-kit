using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.ViewModels
{
    public class UsersViewModel
    {
        public IEnumerable<UserViewModel> Users = new List<UserViewModel>();
    }
}
