using CleanBlazor.Domain.Abstractions;

namespace CleanBlazor.Domain.Primitives;

public abstract class Entity<TId> : IEquatable<Entity<TId>>, IEntity<TId>
{
    protected Entity(TId id) : this()
    {
        Id = id;
    }
    protected Entity() {}
    public TId Id { get; private set; }

    public static bool operator ==(Entity<TId> a, Entity<TId> b)
    {
        if (a is null && b is null)
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Equals(b);
    }

    public static bool operator !=(Entity<TId> a, Entity<TId> b) => !(a == b);

    public bool Equals(Entity<TId> other)
    {
        if (other is null)
        {
            return false;
        }

        return ReferenceEquals(this, other) || Id.Equals(other.Id);
    }

    public override bool Equals(object obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        if (obj is not Entity<TId> other)
        {
            return false;
        }

        if (EqualityComparer<TId>.Default.Equals(Id, default)
            || EqualityComparer<TId>.Default.Equals(other.Id, default))
        {
            return false;
        }

        return EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    public override int GetHashCode() => Id.GetHashCode() * 41;
}
