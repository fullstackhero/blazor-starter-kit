using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Features.Brands.Queries.GetAllCached;
using BlazorHero.CleanArchitecture.Application.Features.Brands.Queries.GetById;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;

namespace BlazorHero.CleanArchitecture.Application.Mappings
{
    public class BrandProfile : Profile
    {
        public BrandProfile()
        {
            //CreateMap<CreateBrandCommand, Brand>().ReverseMap();
            CreateMap<GetBrandByIdResponse, Brand>().ReverseMap();
            CreateMap<GetAllBrandsResponse, Brand>().ReverseMap();
        }
    }
}