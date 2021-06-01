namespace BlazorHero.CleanArchitecture.Domain.Entities
{
    public interface IEntity<TId> : IEntity
    {
        public TId Id { get; set; }
    }

    public interface IEntity
    {
    }
}