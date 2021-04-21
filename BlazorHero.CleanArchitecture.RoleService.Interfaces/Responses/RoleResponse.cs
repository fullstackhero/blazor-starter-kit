using System.ComponentModel.DataAnnotations;

namespace BlazorHero.CleanArchitecture.RoleService.Interfaces.Responses
{
    public class RoleResponse
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}