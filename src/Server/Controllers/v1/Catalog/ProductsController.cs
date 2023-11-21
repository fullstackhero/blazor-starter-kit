using CleanBlazor.Application.Features.Catalog.Products.Commands;
using CleanBlazor.Application.Features.Catalog.Products.Queries;
using CleanBlazor.Contracts.Catalog.Products;
using CleanBlazor.Shared.Constants.Permission;
using CleanBlazor.Shared.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanBlazor.Server.Controllers.v1.Catalog;

public class ProductsController : BaseApiController
{
    /// <summary>
    ///     Get All Products
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="searchString"></param>
    /// <param name="orderBy"></param>
    /// <returns>Status 200 OK</returns>
    [Authorize(Policy = Permissions.Products.View)]
    [HttpGet]
    public async Task<IActionResult> GetAll(int pageNumber, int pageSize, string searchString, string orderBy = null)
    {
        PaginatedResult<GetAllPagedProductsResponse> products =
            await Sender.Send(new GetAllProductsQuery(pageNumber, pageSize, searchString, orderBy));
        return Ok(products);
    }

    /// <summary>
    ///     Get a Product Image by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Status 200 OK</returns>
    [Authorize(Policy = Permissions.Products.View)]
    [HttpGet("image/{id:int}")]
    public async Task<IActionResult> GetProductImageAsync(int id)
    {
        Result<string> result = await Sender.Send(new GetProductImageQuery(id));
        return Ok(result);
    }

    /// <summary>
    ///     Add/Edit a Product
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Status 200 OK</returns>
    [Authorize(Policy = Permissions.Products.Create)]
    [HttpPost]
    public async Task<IActionResult> Post(AddEditProductRequest request)
    {
        var command = new AddEditProductCommand
        {
            Id = request.Id,
            Name = request.Name,
            Barcode = request.Barcode,
            Description = request.Description,
            Rate = request.Rate,
            BrandId = request.BrandId,
            UploadRequest = request.UploadRequest,
            ImageDataUrl = request.ImageDataUrl
        };
        return Ok(await Sender.Send(command));
    }

    /// <summary>
    ///     Delete a Product
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Status 200 OK response</returns>
    [Authorize(Policy = Permissions.Products.Delete)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) => Ok(await Sender.Send(new DeleteProductCommand(id)));

    /// <summary>
    ///     Search Products and Export to Excel
    /// </summary>
    /// <param name="searchString"></param>
    /// <returns>Status 200 OK</returns>
    [Authorize(Policy = Permissions.Products.Export)]
    [HttpGet("export")]
    public async Task<IActionResult> Export(string searchString = "") =>
        Ok(await Sender.Send(new ExportProductsQuery(searchString)));
}
