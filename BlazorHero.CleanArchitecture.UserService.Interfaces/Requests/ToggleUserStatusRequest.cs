namespace BlazorHero.CleanArchitecture.UserService.Interfaces.Requests
{
    public class ToggleUserStatusRequest
    {
        public bool ActivateUser { get; set; }
        public string UserId { get; set; }
    }
}