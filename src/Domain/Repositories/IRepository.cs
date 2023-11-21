using CleanBlazor.Domain.Abstractions;

namespace CleanBlazor.Domain.Repositories;

public interface IRepository<T, in TId>
    where T : IEntity<TId>
{
    IQueryable<T> Entities { get; }
    Task<T> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

    Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<List<T>> GetPagedResponseAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);

    T Add(T entity);

    void AddRange(IEnumerable<T> entities);

    void Update(T entity);

    T Remove(T entity);
}
