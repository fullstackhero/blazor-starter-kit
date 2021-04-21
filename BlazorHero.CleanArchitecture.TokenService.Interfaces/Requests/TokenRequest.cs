using System.ComponentModel.DataAnnotations;

namespace BlazorHero.CleanArchitecture.TokenService.Interfaces.Requests
{
    public class TokenRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}