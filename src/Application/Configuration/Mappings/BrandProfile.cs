using AutoMapper;
using CleanBlazor.Application.Features.Catalog.Brands.Commands;
using CleanBlazor.Contracts.Catalog.Brands;
using CleanBlazor.Domain.Entities.Catalog;

namespace CleanBlazor.Application.Configuration.Mappings;

public class BrandProfile : Profile
{
    public BrandProfile()
    {
        CreateMap<AddEditBrandCommand, Brand>().ReverseMap();
        CreateMap<GetBrandByIdResponse, Brand>().ReverseMap();
        CreateMap<GetAllBrandsResponse, Brand>().ReverseMap();
    }
}
