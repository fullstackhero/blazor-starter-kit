using System;

namespace BlazorHero.CleanArchitecture.Domain.Contracts
{
    public abstract class DeletableEntity : AuditableEntity, IDeletableEntity
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}