using BlazorHero.CleanArchitecture.Application.Specifications.Base;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;

namespace BlazorHero.CleanArchitecture.Application.Specifications
{
    public class ProductFilterSpecification : HeroSpecification<Product>
    {
        public ProductFilterSpecification(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.IsDeleted == false;
            }
            else
            {
                Criteria = p => p.IsDeleted == false &&
                          (p.Name.Contains(searchString) || p.Description.Contains(searchString) || p.Barcode.Contains(searchString) || p.Brand.Name.Contains(searchString));
            }

        }
    }
}
