using System.Collections.Generic;

namespace BlazorHero.CleanArchitecture.Domain.Contracts
{
    public interface IEntityWithExtendedAttributes<TExtendedAttribute>
    {
        public ICollection<TExtendedAttribute> ExtendedAttributes { get; set; }
    }
}