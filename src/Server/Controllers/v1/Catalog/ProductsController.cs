using BlazorHero.CleanArchitecture.Application.Features.Products.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.Products.Commands.Delete;
using BlazorHero.CleanArchitecture.Application.Features.Products.Queries.Export;
using BlazorHero.CleanArchitecture.Application.Features.Products.Queries.GetAllPaged;
using BlazorHero.CleanArchitecture.Application.Features.Products.Queries.GetProductImage;
using BlazorHero.CleanArchitecture.Application.Requests.Catalog;
using BlazorHero.CleanArchitecture.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Server.Controllers.v1.Catalog
{
    public class ProductsController : BaseApiController<ProductsController>
    {


    /// <summary>
    /// Get Products Paged Data
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [Authorize(Policy = Permissions.Products.View)]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery]GetAllPagedProductsRequest request)
    {
      var products = await _mediator.Send(new GetAllProductsQuery(request.PageNumber,request.PageSize,request.SearchString,request.Orderby));
      return Ok(products);
    }

    /// <summary>
    /// Get a Product Image by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Status 200 OK</returns>
    [Authorize(Policy = Permissions.Products.View)]
        [HttpGet("image/{id}")]
        public async Task<IActionResult> GetProductImageAsync(int id)
        {
            var result = await _mediator.Send(new GetProductImageQuery(id));
            return Ok(result);
        }

        /// <summary>
        /// Add/Edit a Product
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Products.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditProductCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Product
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.Products.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteProductCommand { Id = id }));
        }
    /// <summary>
    /// Delete Checked Products
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize(Policy = Permissions.Products.Delete)]
    [HttpDelete()]
    public async Task<IActionResult> DeleteChecked([FromQuery] int[] id)
    {
      return Ok(await _mediator.Send(new DeleteCheckedProductCommand { Id = id }));
    }
    /// <summary>
    /// Search Products and Export to Excel
    /// </summary>
    /// <param name="searchString"></param>
    /// <returns>Status 200 OK</returns>
    [Authorize(Policy = Permissions.Products.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportProductsQuery(searchString)));
        }
    }
}