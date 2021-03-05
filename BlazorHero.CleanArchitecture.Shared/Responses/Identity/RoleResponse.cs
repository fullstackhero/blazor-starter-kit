using System.ComponentModel.DataAnnotations;

namespace BlazorHero.CleanArchitecture.Shared.Responses.Identity
{
    public class RoleResponse
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
