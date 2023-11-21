namespace CleanBlazor.Contracts.Documents;

public class GetAllDocumentsResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsPublic { get; set; }
    public string CreatedBy { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string Url { get; set; }
    public string DocumentType { get; set; }
    public int DocumentTypeId { get; set; }
}
