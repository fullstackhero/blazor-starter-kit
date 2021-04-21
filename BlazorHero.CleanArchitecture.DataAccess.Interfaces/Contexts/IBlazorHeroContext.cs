using System.Threading;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Domain.Entities.Chat;
using BlazorHero.CleanArchitecture.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlazorHero.CleanArchitecture.DataAccess.Interfaces.Contexts
{
    public interface IBlazorHeroContext : IAuditableContext

    {
        public DbSet<BlazorHeroUser> Users { get; set; }

        public DbSet<ChatHistory> ChatHistories { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new());
    }
}
