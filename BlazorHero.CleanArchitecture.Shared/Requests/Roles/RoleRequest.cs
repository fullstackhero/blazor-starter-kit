using System.ComponentModel.DataAnnotations;

namespace BlazorHero.CleanArchitecture.Shared.Requests.Roles
{
    public class RoleRequest
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
