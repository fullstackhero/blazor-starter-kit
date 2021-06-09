using BlazorHero.CleanArchitecture.Application.Interfaces.Serialization.Serializers;
using FluentValidation;

namespace BlazorHero.CleanArchitecture.Application.Validators.Extensions
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilderOptions<T, string> MustBeJson<T>(this IRuleBuilder<T, string> ruleBuilder, IJsonSerializer jsonSerializer) where T : class
        {
            return ruleBuilder.SetValidator(new JsonValidator<T>(jsonSerializer));
        }
    }
}