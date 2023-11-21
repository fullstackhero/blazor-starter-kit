namespace CleanBlazor.Contracts.Catalog.Products;

public class GetAllPagedProductsRequest : PagedRequest
{
    public string SearchString { get; set; }
}
