using CleanBlazor.Domain.Entities.Misc;
using CleanBlazor.Domain.Repositories;
using CleanBlazor.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CleanBlazor.Infrastructure.Repositories;

internal sealed class DocumentRepository : GenericRepository<Document, int>, IDocumentRepository
{
    public DocumentRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> IsDocumentTypeUsedAsync(int documentTypeId) =>
        await Entities.AnyAsync(b => b.DocumentTypeId == documentTypeId);
}
