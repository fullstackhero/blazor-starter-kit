using AutoMapper;
using CleanBlazor.Application.Features.Catalog.Products.Commands;
using CleanBlazor.Domain.Entities.Catalog;

namespace CleanBlazor.Application.Configuration.Mappings;

public class ProductProfile : Profile
{
    public ProductProfile() => CreateMap<AddEditProductCommand, Product>().ReverseMap();
}
