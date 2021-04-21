namespace BlazorHero.CleanArchitecture.RoleService.Interfaces.Requests
{
    public class RoleClaimsRequest
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public bool Selected { get; set; }
    }
}