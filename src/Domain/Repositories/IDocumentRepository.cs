using CleanBlazor.Domain.Entities.Misc;

namespace CleanBlazor.Domain.Repositories;

public interface IDocumentRepository : IRepository<Document, int>
{
    Task<bool> IsDocumentTypeUsedAsync(int documentTypeId);
}
