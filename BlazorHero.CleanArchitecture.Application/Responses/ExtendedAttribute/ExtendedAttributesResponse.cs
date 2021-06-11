using System.Collections.Generic;
using BlazorHero.CleanArchitecture.Application.Features.ExtendedAttributes.Queries.GetAllByEntityId;

namespace BlazorHero.CleanArchitecture.Application.Responses.ExtendedAttribute
{
    public class ExtendedAttributesResponse<TId, TEntityId>
    {
        public TEntityId EntityId { get; set; }
        public string EntityName { get; set; }
        public List<GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId>> ExtendedAttributes { get; set; }
    }
}