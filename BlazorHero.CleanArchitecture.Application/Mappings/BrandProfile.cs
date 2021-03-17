using AutoMapper;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;

namespace BlazorHero.CleanArchitecture.Application.Mappings
{
    public class BrandProfile : Profile
    {
        public BrandProfile()
        {
            //CreateMap<CreateBrandCommand, Brand>().ReverseMap();
            //CreateMap<GetBrandByIdResponse, Brand>().ReverseMap();
            //CreateMap<GetAllBrandsCachedResponse, Brand>().ReverseMap();
        }
    }
}