using System.ComponentModel.DataAnnotations;

namespace BlazorHero.CleanArchitecture.UserService.Interfaces.Requests
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}