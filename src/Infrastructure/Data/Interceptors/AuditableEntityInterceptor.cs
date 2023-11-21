using CleanBlazor.Application.Abstractions.Infrastructure.Services;
using CleanBlazor.Application.Enums;
using CleanBlazor.Domain.Abstractions;
using CleanBlazor.Infrastructure.Models.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CleanBlazor.Infrastructure.Data.Interceptors;

public class AuditableEntityInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserService _currentUserService;
    private readonly TimeProvider _timeProvider;

    private readonly List<AuditEntry> _addedAuditEntries = new();

    public AuditableEntityInterceptor(
        ICurrentUserService currentUserService,
        TimeProvider timeProvider)
    {
        _currentUserService = currentUserService;
        _timeProvider = timeProvider;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        SetAuditProperties(eventData.Context);
        AddAuditTrails(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        SetAuditProperties(eventData.Context);
        AddAuditTrails(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        UpdateAuditTrailEntities(eventData.Context);

        return base.SavedChanges(eventData, result);
    }

    public override ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        UpdateAuditTrailEntities(eventData.Context);

        return base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private void SetAuditProperties(DbContext context)
    {
        if (context == null) return;

        var now = _timeProvider.GetUtcNow();

        foreach (var entry in context.ChangeTracker.Entries<IAuditableEntity>())
        {
            if (entry.State is EntityState.Added)
            {
                entry.Property(nameof(IAuditableEntity.CreatedBy)).CurrentValue = _currentUserService.UserId;
                entry.Property(nameof(IAuditableEntity.CreatedOn)).CurrentValue = now;
            }

            if (entry.State is EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                entry.Property(nameof(IAuditableEntity.LastModifiedBy)).CurrentValue = _currentUserService.UserId;
                entry.Property(nameof(IAuditableEntity.LastModifiedOn)).CurrentValue = now;
            }
        }
    }

    private void AddAuditTrails(DbContext context)
    {
        if (context is not ApplicationDbContext applicationDbContext)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(_currentUserService.UserId))
        {
            return;
        }

        _addedAuditEntries.Clear();

        foreach (var entry in context.ChangeTracker.Entries<IAuditableEntity>())
        {
            if (entry.State is EntityState.Detached or EntityState.Unchanged)
            {
                continue;
            }

            var auditEntry = new AuditEntry(entry)
            {
                TableName = entry.Entity.GetType().Name,
                UserId = _currentUserService.UserId
            };
            _addedAuditEntries.Add(auditEntry);
            ProcessAuditEntryProperties(entry, auditEntry);
        }

        var now = _timeProvider.GetUtcNow();
        foreach (AuditEntry auditEntry in _addedAuditEntries.Where(e => !e.HasTemporaryProperties))
        {
            applicationDbContext.AuditTrails.Add(auditEntry.ToAudit(now));
        }
    }

    private static void ProcessAuditEntryProperties(EntityEntry entry, AuditEntry auditEntry)
    {
        foreach (PropertyEntry property in entry.Properties)
        {
            var propertyName = property.Metadata.Name;
            if (property.IsTemporary)
            {
                auditEntry.TemporaryProperties.Add(property);
                continue;
            }

            if (property.Metadata.IsPrimaryKey())
            {
                auditEntry.KeyValues[propertyName] = property.CurrentValue;
                continue;
            }

            ProcessAuditEntryState(entry.State, property, auditEntry, propertyName);
        }
    }

    private static void ProcessAuditEntryState(
        EntityState state,
        PropertyEntry property,
        AuditEntry auditEntry,
        string propertyName)
    {
        switch (state)
        {
            case EntityState.Added:
                auditEntry.AuditType = AuditType.Create;
                auditEntry.NewValues[propertyName] = property.CurrentValue;
                break;

            case EntityState.Deleted:
                auditEntry.AuditType = AuditType.Delete;
                auditEntry.OldValues[propertyName] = property.OriginalValue;
                break;

            case EntityState.Modified:
                if (property.IsModified && property.OriginalValue?.Equals(property.CurrentValue) == false)
                {
                    auditEntry.ChangedColumns.Add(propertyName);
                    auditEntry.AuditType = AuditType.Update;
                    auditEntry.OldValues[propertyName] = property.OriginalValue;
                    auditEntry.NewValues[propertyName] = property.CurrentValue;
                }

                break;

            case EntityState.Detached:
            case EntityState.Unchanged:
            default:
                break;
        }
    }

    private void UpdateAuditTrailEntities(DbContext context)
    {
        if (context is not ApplicationDbContext applicationDbContext)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(_currentUserService.UserId))
        {
            return;
        }

        if (!_addedAuditEntries.Any())
        {
            return;
        }

        var now = _timeProvider.GetUtcNow();
        foreach (var auditEntry in _addedAuditEntries.Where(x => x.HasTemporaryProperties))
        {
            foreach (PropertyEntry prop in auditEntry.TemporaryProperties)
            {
                if (prop.Metadata.IsPrimaryKey())
                {
                    auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                }
                else
                {
                    auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                }
            }

            applicationDbContext.AuditTrails.Add(auditEntry.ToAudit(now));
        }

        _addedAuditEntries.Clear();
    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            r.TargetEntry.State is EntityState.Added or EntityState.Modified);
}
