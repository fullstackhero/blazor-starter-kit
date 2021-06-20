using System;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Features.ExtendedAttributes.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.ExtendedAttributes.Commands.Delete;
using BlazorHero.CleanArchitecture.Application.Features.ExtendedAttributes.Queries.Export;
using BlazorHero.CleanArchitecture.Application.Features.ExtendedAttributes.Queries.GetAll;
using BlazorHero.CleanArchitecture.Application.Features.ExtendedAttributes.Queries.GetAllByEntityId;
using BlazorHero.CleanArchitecture.Application.Features.ExtendedAttributes.Queries.GetById;
using BlazorHero.CleanArchitecture.Domain.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BlazorHero.CleanArchitecture.Server.Controllers.Utilities.ExtendedAttributes.Base
{
    /// <summary>
    /// Abstract Extended Attributes Controller Class
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public abstract class ExtendedAttributesController<TId, TEntityId, TEntity, TExtendedAttribute>
        : BaseApiController<ExtendedAttributesController<TId, TEntityId, TEntity, TExtendedAttribute>>
            where TEntity : AuditableEntity<TEntityId>, IEntityWithExtendedAttributes<TExtendedAttribute>, IEntity<TEntityId>
            where TExtendedAttribute : AuditableEntityExtendedAttribute<TId, TEntityId, TEntity>, IEntity<TId>
            where TId : IEquatable<TId>
    {
        /// <summary>
        /// Get All Entity Extended Attributes
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [HttpGet]
        public virtual async Task<IActionResult> GetAll()
        {
            var extendedAttributes = await _mediator.Send(new GetAllExtendedAttributesQuery<TId, TEntityId, TEntity, TExtendedAttribute>());
            return Ok(extendedAttributes);
        }

        /// <summary>
        /// Get All Entity Extended Attributes by entity id
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns>Status 200 OK</returns>
        [HttpGet("by-entity/{entityId}")]
        public virtual async Task<IActionResult> GetAllByEntityId(TEntityId entityId)
        {
            var extendedAttributes = await _mediator.Send(new GetAllExtendedAttributesByEntityIdQuery<TId, TEntityId, TEntity, TExtendedAttribute>(entityId));
            return Ok(extendedAttributes);
        }

        /// <summary>
        /// Get Entity Extended Attribute By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(TId id)
        {
            var extendedAttribute = await _mediator.Send(new GetExtendedAttributeByIdQuery<TId, TEntityId, TEntity, TExtendedAttribute> { Id = id });
            return Ok(extendedAttribute);
        }

        /// <summary>
        /// Create/Update a Entity Extended Attribute
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [HttpPost]
        public virtual async Task<IActionResult> Post(AddEditExtendedAttributeCommand<TId, TEntityId, TEntity, TExtendedAttribute> command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Entity Extended Attribute
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(TId id)
        {
            return Ok(await _mediator.Send(new DeleteExtendedAttributeCommand<TId, TEntityId, TEntity, TExtendedAttribute> { Id = id }));
        }

        /// <summary>
        /// Search Entity Extended Attribute and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="entityId"></param>
        /// <param name="includeEntity"></param>
        /// <param name="onlyCurrentGroup"></param>
        /// <param name="currentGroup"></param>
        /// <returns></returns>
        [HttpGet("export")]
        public virtual async Task<IActionResult> Export(string searchString = "", TEntityId entityId = default, bool includeEntity = false, bool onlyCurrentGroup = false, string currentGroup = "")
        {
            return Ok(await _mediator.Send(new ExportExtendedAttributesQuery<TId, TEntityId, TEntity, TExtendedAttribute>(searchString, entityId, includeEntity, onlyCurrentGroup, currentGroup)));
        }
    }
}