using BlazorHero.CleanArchitecture.Application.Features.Organization.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Validation.Features.Organization.Commands.AddEdit
{
   
    public class AddEditCompanyCommandValidator : AbstractValidator<AddEditCompanyCommand>
    {
        public AddEditCompanyCommandValidator(IStringLocalizer<AddEditCompanyCommandValidator> localizer)
        {
            var msg = localizer["Code maxlength is 10"];
            RuleFor(v => v.Code)
               .MaximumLength(10).WithMessage(m=>localizer["Code maxlength is 10"])
               .NotEmpty().WithMessage(m=>localizer["Code is required"]);
            RuleFor(v => v.Name)
                .MaximumLength(200).WithMessage(m => localizer["Name maxlength is 200"])
                .NotEmpty().WithMessage(m => localizer["Name is required"]); ;
            RuleFor(v => v.Manager)
                .MaximumLength(12).WithMessage(m => localizer["Manager maxlenght is 12"]);
        }
    }
}
 

