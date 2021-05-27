namespace BlazorHero.CleanArchitecture.Application.Responses.Identity
{
    public class RoleClaimsResponse
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public string Group { get; set; }
        public bool Selected { get; set; }
    }
}