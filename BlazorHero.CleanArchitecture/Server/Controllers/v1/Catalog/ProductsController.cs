using BlazorHero.CleanArchitecture.Application.Features.Products.Queries.GetAllPaged;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Server.Controllers.v1.Catalog
{
    public class ProductsController : BaseApiController<ProductsController>
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize, string searchString)
        {
            var products = await _mediator.Send(new GetAllProductsQuery(pageNumber, pageSize, searchString));
            return Ok(products);
        }
    }
}
