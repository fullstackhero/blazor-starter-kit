using System;

namespace BlazorHero.CleanArchitecture.Application.Contracts
{
    public class DeletableEntity : AuditableEntity, IDeletableEntity
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}