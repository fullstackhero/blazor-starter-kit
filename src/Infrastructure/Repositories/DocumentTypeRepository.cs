using CleanBlazor.Domain.Entities.Misc;
using CleanBlazor.Domain.Repositories;
using CleanBlazor.Infrastructure.Data;

namespace CleanBlazor.Infrastructure.Repositories;

internal sealed class DocumentTypeRepository : GenericRepository<DocumentType, int>, IDocumentTypeRepository
{
    public DocumentTypeRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
