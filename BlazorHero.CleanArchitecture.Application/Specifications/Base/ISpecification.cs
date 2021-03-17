using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BlazorHero.CleanArchitecture.Application.Specifications.Base
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        List<string> IncludeStrings { get; }
    }
}