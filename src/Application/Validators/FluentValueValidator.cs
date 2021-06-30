using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Validation
{
    public class FluentValueValidator<T> : AbstractValidator<T>
    {
        public FluentValueValidator(Action<IRuleBuilderInitial<T, T>> rule)
        {
            rule(RuleFor(x => x));
        }

        private IEnumerable<string> ValidateValue(T arg)
        {
            var result = Validate(arg);
            if (result.IsValid)
                return new string[0];
            return result.Errors.Select(e => e.ErrorMessage);
        }

        public Func<T, IEnumerable<string>> Validation => ValidateValue;
    }
}
