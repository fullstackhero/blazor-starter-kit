using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Features.DocumentExtendedAttributes.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.DocumentExtendedAttributes.Commands.Delete;
using BlazorHero.CleanArchitecture.Application.Features.DocumentExtendedAttributes.Queries.Export;
using BlazorHero.CleanArchitecture.Application.Features.DocumentExtendedAttributes.Queries.GetAll;
using BlazorHero.CleanArchitecture.Application.Features.DocumentExtendedAttributes.Queries.GetById;
using BlazorHero.CleanArchitecture.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorHero.CleanArchitecture.Server.Controllers.Utilities.Misc
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentExtendedAttributesController : BaseApiController<DocumentExtendedAttributesController>
    {
        /// <summary>
        /// Get All Document Extended Attributes
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.DocumentExtendedAttributes.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var documentTypes = await _mediator.Send(new GetAllDocumentExtendedAttributesQuery());
            return Ok(documentTypes);
        }

        /// <summary>
        /// Get Document Extended Attribute By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.DocumentExtendedAttributes.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var documentExtendedAttribute = await _mediator.Send(new GetDocumentExtendedAttributeByIdQuery { Id = id });
            return Ok(documentExtendedAttribute);
        }

        /// <summary>
        /// Create/Update a Document Extended Attribute
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.DocumentExtendedAttributes.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditDocumentExtendedAttributeCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Document Extended Attribute
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.DocumentExtendedAttributes.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteDocumentExtendedAttributeCommand { Id = id }));
        }

        /// <summary>
        /// Search Document Extended Attribute and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="includeDocument"></param>
        /// <param name="includeDocumentType"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.DocumentExtendedAttributes.View)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "", bool includeDocument = false, bool includeDocumentType = false)
        {
            return Ok(await _mediator.Send(new ExportDocumentExtendedAttributesQuery(searchString, includeDocument, includeDocumentType)));
        }
    }
}