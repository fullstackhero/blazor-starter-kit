using System.ComponentModel.DataAnnotations;

namespace BlazorHero.CleanArchitecture.Client.ViewModels.Identity
{
    public class ProfileViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public char FirstLetterOfName { get; set; }
    }
}