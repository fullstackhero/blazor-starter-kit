using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using BlazorHero.CleanArchitecture.Domain.Contracts;

namespace BlazorHero.CleanArchitecture.Application.Specifications.Base
{
    public interface ISpecification<T, in TId> where T : class, IEntity<TId>
    {
        Expression<Func<T, bool>> Criteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        List<string> IncludeStrings { get; }
    }
}