using AutoMapper;
using CleanBlazor.Contracts.Audit;
using CleanBlazor.Infrastructure.Models.Audit;

namespace CleanBlazor.Infrastructure.Configuration.Mappings;

public class AuditProfile : Profile
{
    public AuditProfile() => CreateMap<AuditResponse, Audit>().ReverseMap();
}
