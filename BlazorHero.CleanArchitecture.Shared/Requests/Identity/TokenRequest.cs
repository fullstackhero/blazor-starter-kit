using System.ComponentModel.DataAnnotations;

namespace BlazorHero.CleanArchitecture.Shared.Requests.Identity
{
    public class TokenRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}