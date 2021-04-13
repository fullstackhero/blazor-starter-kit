using BlazorHero.CleanArchitecture.Domain.Contracts;

namespace BlazorHero.CleanArchitecture.Domain.Entities
{
    public class Document : AuditableEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Extension { get; set; }
        public bool IsPublic { get; set; } = false;
        public byte[] Data { get; set; }
    }
}
