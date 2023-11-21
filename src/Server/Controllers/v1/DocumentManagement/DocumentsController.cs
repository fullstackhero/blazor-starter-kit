using CleanBlazor.Application.Features.DocumentManagement.Documents.Commands;
using CleanBlazor.Application.Features.DocumentManagement.Documents.Queries;
using CleanBlazor.Contracts.Documents;
using CleanBlazor.Shared.Constants.Permission;
using CleanBlazor.Shared.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanBlazor.Server.Controllers.v1.DocumentManagement;

public class DocumentsController : BaseApiController
{
    /// <summary>
    ///     Get All Documents
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="searchString"></param>
    /// <returns>Status 200 OK</returns>
    [Authorize(Policy = Permissions.Documents.View)]
    [HttpGet]
    public async Task<IActionResult> GetAll(int pageNumber, int pageSize, string searchString)
    {
        PaginatedResult<GetAllDocumentsResponse> docs =
            await Sender.Send(new GetAllDocumentsQuery(pageNumber, pageSize, searchString));
        return Ok(docs);
    }

    /// <summary>
    ///     Get Document By Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Status 200 Ok</returns>
    [Authorize(Policy = Permissions.Documents.View)]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        Result<GetDocumentByIdResponse> document = await Sender.Send(new GetDocumentByIdQuery(id));
        return Ok(document);
    }

    /// <summary>
    ///     Add/Edit Document
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Status 200 OK</returns>
    [Authorize(Policy = Permissions.Documents.Create)]
    [HttpPost]
    public async Task<IActionResult> Post(AddEditDocumentRequest request)
    {
        var command = new AddEditDocumentCommand
        {
            Id = request.Id,
            Title = request.Title,
            Description = request.Description,
            IsPublic = request.IsPublic,
            Url = request.Url,
            DocumentTypeId = request.DocumentTypeId,
            UploadRequest = request.UploadRequest
        };
        return Ok(await Sender.Send(command));
    }

    /// <summary>
    ///     Delete a Document
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Status 200 OK</returns>
    [Authorize(Policy = Permissions.Documents.Delete)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) => Ok(await Sender.Send(new DeleteDocumentCommand(id)));
}
