namespace BlazorHero.CleanArchitecture.Application.Features.Brands.Queries.GetById
{
    public class GetBrandByIdResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Tax { get; set; }
        public string Description { get; set; }
    }
}