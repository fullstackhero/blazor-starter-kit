using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Features.Products.Commands.Create;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;

namespace BlazorHero.CleanArchitecture.Application.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<CreateProductCommand, Product>().ReverseMap();
        }
    }
}