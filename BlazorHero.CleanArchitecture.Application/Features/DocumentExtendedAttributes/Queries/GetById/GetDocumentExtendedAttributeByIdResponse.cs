#nullable enable
using System;
using BlazorHero.CleanArchitecture.Domain.Enums;

namespace BlazorHero.CleanArchitecture.Application.Features.DocumentExtendedAttributes.Queries.GetById
{
    public class GetDocumentExtendedAttributeByIdResponse
    {
        public int Id { get; set; }
        public int EntityId { get; set; }
        public EntityExtendedAttributeType Type { get; set; }
        public string Key { get; set; }
        public string? Text { get; set; }
        public decimal? Decimal { get; set; }
        public DateTime? DateTime { get; set; }
        public string? Json { get; set; }
        public string? ExternalId { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}