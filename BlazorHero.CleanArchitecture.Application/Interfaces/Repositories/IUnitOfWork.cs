using BlazorHero.CleanArchitecture.Domain.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepositoryAsync<TEntity> Repository<TEntity>() where TEntity : AuditableEntity;
        Task<int> Commit(CancellationToken cancellationToken);
        Task Rollback();
    }
}