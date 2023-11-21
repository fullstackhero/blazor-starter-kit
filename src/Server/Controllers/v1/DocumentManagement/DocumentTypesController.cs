using CleanBlazor.Application.Features.DocumentManagement.DocumentTypes.Commands;
using CleanBlazor.Application.Features.DocumentManagement.DocumentTypes.Queries;
using CleanBlazor.Contracts.Documents;
using CleanBlazor.Shared.Constants.Permission;
using CleanBlazor.Shared.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanBlazor.Server.Controllers.v1.DocumentManagement;

public class DocumentTypesController : BaseApiController
{
    /// <summary>
    ///     Get All Document Types
    /// </summary>
    /// <returns>Status 200 OK</returns>
    [Authorize(Policy = Permissions.DocumentTypes.View)]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        Result<List<GetAllDocumentTypesResponse>> documentTypes = await Sender.Send(new GetAllDocumentTypesQuery());
        return Ok(documentTypes);
    }

    /// <summary>
    ///     Get Document Type By Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Status 200 Ok</returns>
    [Authorize(Policy = Permissions.DocumentTypes.View)]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        Result<GetDocumentTypeByIdResponse> documentType = await Sender.Send(new GetDocumentTypeByIdQuery(id));
        return Ok(documentType);
    }

    /// <summary>
    ///     Create/Update a Document Type
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Status 200 OK</returns>
    [Authorize(Policy = Permissions.DocumentTypes.Create)]
    [HttpPost]
    public async Task<IActionResult> Post(AddEditDocumentTypeRequest request)
    {
        var command = new AddEditDocumentTypeCommand(request.Id, request.Name, request.Description);
        return Ok(await Sender.Send(command));
    }

    /// <summary>
    ///     Delete a Document Type
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Status 200 OK</returns>
    [Authorize(Policy = Permissions.DocumentTypes.Delete)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) =>
        Ok(await Sender.Send(new DeleteDocumentTypeCommand(id)));

    /// <summary>
    ///     Search Document Types and Export to Excel
    /// </summary>
    /// <param name="searchString"></param>
    /// <returns></returns>
    [Authorize(Policy = Permissions.DocumentTypes.Export)]
    [HttpGet("export")]
    public async Task<IActionResult> Export(string searchString = "") =>
        Ok(await Sender.Send(new ExportDocumentTypesQuery(searchString)));
}
