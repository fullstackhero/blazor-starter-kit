using System.ComponentModel.DataAnnotations;

namespace BlazorHero.CleanArchitecture.Application.Requests.Roles
{
    public class RoleRequest
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
