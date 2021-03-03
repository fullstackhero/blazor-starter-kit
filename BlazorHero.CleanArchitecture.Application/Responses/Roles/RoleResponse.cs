using System.ComponentModel.DataAnnotations;

namespace BlazorHero.CleanArchitecture.Application.Responses.Roles
{
    public class RoleResponse
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
