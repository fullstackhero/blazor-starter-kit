using AutoMapper;
using BlazorHero.CleanArchitecture.Infrastructure.Models.Audit;
using BlazorHero.CleanArchitecture.Application.Responses.Audit;

namespace BlazorHero.CleanArchitecture.Infrastructure.Mappings
{
    public class AuditProfile : Profile
    {
        public AuditProfile()
        {
            CreateMap<AuditResponse, Audit>().ReverseMap();
        }
    }
}