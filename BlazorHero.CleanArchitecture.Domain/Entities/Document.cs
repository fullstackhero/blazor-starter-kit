using BlazorHero.CleanArchitecture.Domain.Contracts;

namespace BlazorHero.CleanArchitecture.Domain.Entities
{
    public class Document : AuditableEntity<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; } = false;
        public string URL { get; set; }
    }
}