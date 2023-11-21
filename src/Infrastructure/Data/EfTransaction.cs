using CleanBlazor.Application.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace CleanBlazor.Infrastructure.Data;

internal sealed class EfTransaction : ITransaction
{
    private readonly IDbContextTransaction _transaction;

    public EfTransaction(IDbContextTransaction transaction)
    {
        _transaction = transaction;
    }

    public Task CommitAsync(CancellationToken cancellationToken = default) =>
        _transaction.CommitAsync(cancellationToken);

    public Task RollbackAsync(CancellationToken cancellationToken = default) =>
        _transaction.RollbackAsync(cancellationToken);

    public void Dispose() => _transaction.Dispose();

    public async ValueTask DisposeAsync() => await _transaction.DisposeAsync();
}
