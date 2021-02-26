using System;

namespace BlazorHero.CleanArchitecture.Application.Contracts
{
    public interface IDeletableEntity
    {
        bool IsDeleted { get; set; }
        DateTime? DeletedOn { get; set; }
    }
}
