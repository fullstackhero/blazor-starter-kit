using CleanBlazor.Application.Abstractions.Infrastructure.Services;
using CleanBlazor.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CleanBlazor.Infrastructure.Data.Interceptors;

public class SoftDeletableEntityInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserService _currentUserService;
    private readonly TimeProvider _timeProvider;

    public SoftDeletableEntityInterceptor(
        ICurrentUserService currentUserService,
        TimeProvider timeProvider)
    {
        _currentUserService = currentUserService;
        _timeProvider = timeProvider;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(DbContext context)
    {
        if (context == null) return;

        var now = _timeProvider.GetUtcNow();

        foreach (var entry in context.ChangeTracker.Entries<ISoftDeletableEntity>())
        {
            if (entry.State != EntityState.Deleted)
            {
                continue;
            }

            entry.Property(nameof(ISoftDeletableEntity.DeletedBy)).CurrentValue = _currentUserService.UserId;
            entry.Property(nameof(ISoftDeletableEntity.DeletedOn)).CurrentValue = now;
            entry.Property(nameof(ISoftDeletableEntity.Deleted)).CurrentValue = true;
            entry.State = EntityState.Modified;

            UpdateDeletedEntityEntryReferencesToUnchanged(entry);
        }
    }

    private static void UpdateDeletedEntityEntryReferencesToUnchanged(EntityEntry entry)
    {
        if (!entry.References.Any())
        {
            return;
        }

        foreach (var targetEntry in entry.References
                     .Select(referenceEntry => referenceEntry.TargetEntry)
                     .Where(targetEntry => targetEntry is { State: EntityState.Deleted }))
        {
            targetEntry.State = EntityState.Unchanged;

            UpdateDeletedEntityEntryReferencesToUnchanged(targetEntry);
        }
    }
}
