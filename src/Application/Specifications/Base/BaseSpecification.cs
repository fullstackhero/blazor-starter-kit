using System.Linq.Expressions;
using CleanBlazor.Application.Extensions;
using CleanBlazor.Domain.Abstractions;

namespace CleanBlazor.Application.Specifications.Base;

public abstract class BaseSpecification<T> : ISpecification<T> where T : class, IEntity
{
    public Expression<Func<T, bool>> Criteria { get; set; }
    public List<Expression<Func<T, object>>> Includes { get; } = new();
    public List<string> IncludeStrings { get; } = new();

    public Expression<Func<T, bool>> And(Expression<Func<T, bool>> query) =>
        Criteria = Criteria == null ? query : Criteria.And(query);

    public Expression<Func<T, bool>> Or(Expression<Func<T, bool>> query) =>
        Criteria = Criteria == null ? query : Criteria.Or(query);

    protected virtual void AddInclude(Expression<Func<T, object>> includeExpression) => Includes.Add(includeExpression);

    protected virtual void AddInclude(string includeString) => IncludeStrings.Add(includeString);
}
