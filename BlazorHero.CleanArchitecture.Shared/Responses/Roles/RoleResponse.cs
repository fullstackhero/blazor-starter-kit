using System.ComponentModel.DataAnnotations;

namespace BlazorHero.CleanArchitecture.Shared.Responses.Roles
{
    public class RoleResponse
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
