namespace BlazorHero.CleanArchitecture.Domain.Contracts
{
    public interface IEntityAuditableExtendedAttribute<TId, TEntityId, TEntity>
        : IEntityExtendedAttribute<TId, TEntityId, TEntity>, IAuditableEntity<TId>
            where TEntity : IEntity<TEntityId>
    {
    }

    public interface IEntityAuditableExtendedAttribute<TEntityId, TEntity>
        : IEntityExtendedAttribute<TEntityId, TEntity>, IAuditableEntity
            where TEntity : IEntity<TEntityId>
    {
    }

    public interface IEntityAuditableExtendedAttribute : IEntityExtendedAttribute, IAuditableEntity
    {
    }
}