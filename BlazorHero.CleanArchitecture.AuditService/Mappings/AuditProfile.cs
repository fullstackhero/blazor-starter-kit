using AutoMapper;
using BlazorHero.CleanArchitecture.AuditService.Interfaces.Responses;
using BlazorHero.CleanArchitecture.Domain.Entities.Audit;

namespace BlazorHero.CleanArchitecture.AuditService.Mappings
{
    public class AuditProfile : Profile
    {
        public AuditProfile()
        {
            CreateMap<AuditResponse, Audit>().ReverseMap();
        }
    }
}