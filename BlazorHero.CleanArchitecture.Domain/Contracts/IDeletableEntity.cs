using System;

namespace BlazorHero.CleanArchitecture.Domain.Contracts
{
    public interface IDeletableEntity
    {
        bool IsDeleted { get; set; }
        DateTime? DeletedOn { get; set; }
    }
}