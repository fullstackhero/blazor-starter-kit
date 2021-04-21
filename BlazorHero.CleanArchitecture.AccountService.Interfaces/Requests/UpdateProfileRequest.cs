using System.ComponentModel.DataAnnotations;

namespace BlazorHero.CleanArchitecture.AccountService.Interfaces.Requests
{
    public class UpdateProfileRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}