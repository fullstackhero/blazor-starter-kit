using BlazorHero.CleanArchitecture.Application.Features.DocumentExtendedAttributes.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Interfaces.Serialization.Serializers;
using BlazorHero.CleanArchitecture.Application.Validators.Extensions;
using BlazorHero.CleanArchitecture.Domain.Enums;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BlazorHero.CleanArchitecture.Application.Validators.Features.DocumentExtendedAttributes.Commands.AddEdit
{
    public class AddEditDocumentExtendedAttributeCommandValidator : AbstractValidator<AddEditDocumentExtendedAttributeCommand>
    {
        public AddEditDocumentExtendedAttributeCommandValidator(IStringLocalizer<AddEditDocumentExtendedAttributeCommandValidator> localizer, IJsonSerializer jsonSerializer)
        {
            RuleFor(request => request.EntityId)
                .GreaterThan(0).WithMessage(x => localizer["Document is required!"]);
            RuleFor(request => request.Key)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Key is required!"]);

            When(request => request.Type == EntityExtendedAttributeType.Decimal, () =>
            {
                RuleFor(request => request.Decimal).NotNull().WithMessage(x => string.Format(localizer["Decimal value is required using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.Text).Null().WithMessage(x => string.Format(localizer["Text value should be null using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.DateTime).Null().WithMessage(x => string.Format(localizer["DateTime value should be null using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.Json).Null().WithMessage(x => string.Format(localizer["Json value should be null using {0} type!"], x.Type.ToString()));
            });

            When(request => request.Type == EntityExtendedAttributeType.Text, () =>
            {
                RuleFor(request => request.Text).NotNull().WithMessage(x => string.Format(localizer["Text value is required using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.Decimal).Null().WithMessage(x => string.Format(localizer["Decimal value should be null using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.DateTime).Null().WithMessage(x => string.Format(localizer["DateTime value should be null using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.Json).Null().WithMessage(x => string.Format(localizer["Json value should be null using {0} type!"], x.Type.ToString()));
            });

            When(request => request.Type == EntityExtendedAttributeType.DateTime, () =>
            {
                RuleFor(request => request.DateTime).NotNull().WithMessage(x => string.Format(localizer["DateTime value is required using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.Text).Null().WithMessage(x => string.Format(localizer["Text value should be null using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.Decimal).Null().WithMessage(x => string.Format(localizer["Decimal value should be null using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.Json).Null().WithMessage(x => string.Format(localizer["Json value should be null using {0} type!"], x.Type.ToString()));
            });

            When(request => request.Type == EntityExtendedAttributeType.Json, () =>
            {
                RuleFor(request => request.Json).MustBeJson(jsonSerializer).WithMessage(x => string.Format(localizer["Json value must be a valid json string using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.Json).NotNull().WithMessage(x => string.Format(localizer["Json value is required using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.Text).Null().WithMessage(x => string.Format(localizer["Text value should be null using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.DateTime).Null().WithMessage(x => string.Format(localizer["DateTime value should be null using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.Decimal).Null().WithMessage(x => string.Format(localizer["Decimal value should be null using {0} type!"], x.Type.ToString()));
            });
        }
    }
}