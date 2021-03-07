using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Server.Controllers.v1
{
    public class BrandController : BaseApiController<BrandController>
    {
        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var brands = await _mediator.Send(new GetAllBrandsCachedQuery());
        //    return Ok(brands);
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetById(int id)
        //{
        //    var brand = await _mediator.Send(new GetBrandByIdQuery() { Id = id });
        //    return Ok(brand);
        //}

        //// POST api/<controller>
        //[HttpPost]
        //public async Task<IActionResult> Post(CreateBrandCommand command)
        //{
        //    return Ok(await _mediator.Send(command));
        //}

        //// PUT api/<controller>/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> Put(int id, UpdateBrandCommand command)
        //{
        //    if (id != command.Id)
        //    {
        //        return BadRequest();
        //    }
        //    return Ok(await _mediator.Send(command));
        //}

        //// DELETE api/<controller>/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    return Ok(await _mediator.Send(new DeleteBrandCommand { Id = id }));
        //}
    }
}
