namespace BlazorHero.CleanArchitecture.Application.Features.Brands.Queries.GetAll
{
    public class GetAllBrandsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Tax { get; set; }
    }
}