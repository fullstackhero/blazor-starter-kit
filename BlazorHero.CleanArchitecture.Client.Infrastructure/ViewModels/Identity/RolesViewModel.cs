using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.ViewModels.Identity
{
    public class RolesViewModel
    {
        public List<RoleViewModel> RoleList = new List<RoleViewModel>();
        public RoleViewModel role = new RoleViewModel();
        public string searchString = "";
    }

    public class RoleViewModel
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}