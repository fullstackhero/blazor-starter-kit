using System.ComponentModel.DataAnnotations;

namespace BlazorHero.CleanArchitecture.AccountService.Interfaces.Requests
{
    public class ChangePasswordRequest
    {
        [Required]
        public string Password { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        public string ConfirmNewPassword { get; set; }
    }
}