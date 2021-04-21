using System.ComponentModel.DataAnnotations;

namespace BlazorHero.CleanArchitecture.RoleService.Interfaces.Requests
{
    public class RoleRequest
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}