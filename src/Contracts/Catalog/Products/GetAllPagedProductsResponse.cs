namespace CleanBlazor.Contracts.Catalog.Products;

public class GetAllPagedProductsResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Barcode { get; set; }
    public string Description { get; set; }
    public decimal Rate { get; set; }
    public string Brand { get; set; }
    public int BrandId { get; set; }
}
