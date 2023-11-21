using System.Linq.Expressions;
using CleanBlazor.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CleanBlazor.Infrastructure.Data.Extensions;

internal static class ModelBuilderExtensions
{
    internal static void ApplyAuditableEntityConfiguration(this ModelBuilder modelBuilder)
    {
        foreach (var clrType in modelBuilder.Model.GetEntityTypes()
                     .Select(entityType => entityType.ClrType)
                     .Where(clrType => typeof(IAuditableEntity).IsAssignableFrom(clrType)))
        {
            modelBuilder.Entity(clrType).Property<DateTimeOffset>(nameof(IAuditableEntity.CreatedOn)).IsRequired();
            modelBuilder.Entity(clrType).Property<string>(nameof(IAuditableEntity.CreatedBy));
            modelBuilder.Entity(clrType).Property<DateTimeOffset?>(nameof(IAuditableEntity.LastModifiedOn));
            modelBuilder.Entity(clrType).Property<string>(nameof(IAuditableEntity.LastModifiedBy));
        }
    }

    internal static void ApplySoftDeletableEntityConfiguration(this ModelBuilder modelBuilder)
    {
        foreach (var clrType in modelBuilder.Model.GetEntityTypes()
                     .Select(entityType => entityType.ClrType)
                     .Where(clrType => typeof(ISoftDeletableEntity).IsAssignableFrom(clrType)))
        {
            modelBuilder.Entity(clrType).Property<DateTimeOffset?>(nameof(ISoftDeletableEntity.DeletedOn));
            modelBuilder.Entity(clrType).Property<string>(nameof(ISoftDeletableEntity.DeletedBy));
            modelBuilder.Entity(clrType).Property<bool>(nameof(ISoftDeletableEntity.Deleted));

            var parameter = Expression.Parameter(clrType, "e");
            var propertyMethodInfo = typeof(EF).GetMethod("Property")?.MakeGenericMethod(typeof(bool));
            if (propertyMethodInfo == null)
            {
                return;
            }

            var isDeletedProperty = Expression.Call(propertyMethodInfo, parameter, Expression.Constant(nameof(ISoftDeletableEntity.Deleted)));
            var notDeletedExpression = Expression.Lambda(Expression.Not(isDeletedProperty), parameter);

            modelBuilder.Entity(clrType).HasQueryFilter(notDeletedExpression);

        }
    }
}
