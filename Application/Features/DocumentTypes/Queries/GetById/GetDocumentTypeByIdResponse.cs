namespace BlazorHero.CleanArchitecture.Application.Features.DocumentTypes.Queries.GetById
{
    public class GetDocumentTypeByIdResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}