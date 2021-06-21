using BlazorHero.CleanArchitecture.Application.Interfaces.Serialization.Serializers;
using FluentValidation;
using FluentValidation.Validators;

namespace BlazorHero.CleanArchitecture.Application.Validators
{
    public class JsonValidator<T> : PropertyValidator<T, string>
    {
        private readonly IJsonSerializer _jsonSerializer;

        public JsonValidator(IJsonSerializer jsonSerializer)
        {
            _jsonSerializer = jsonSerializer;
        }

        public override string Name => "JsonValidator";

        public override bool IsValid(ValidationContext<T> context, string value)
        {
            var isJson = true;
            value = value.Trim();
            try
            {
                _jsonSerializer.Deserialize<object>(value);
            }
            catch
            {
                isJson = false;
            }
            isJson = isJson && value.StartsWith("{") && value.EndsWith("}")
                     || value.StartsWith("[") && value.EndsWith("]");

            return isJson;
        }

        protected override string GetDefaultMessageTemplate(string errorCode)
            => "{PropertyName} must be json string.";
    }
}