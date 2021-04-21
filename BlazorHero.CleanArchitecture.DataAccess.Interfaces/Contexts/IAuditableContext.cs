using BlazorHero.CleanArchitecture.Domain.Entities.Audit;
using Microsoft.EntityFrameworkCore;

namespace BlazorHero.CleanArchitecture.DataAccess.Interfaces.Contexts
{
    public interface IAuditableContext
    {
        public DbSet<Audit> AuditTrails { get; set; }
    }
}
