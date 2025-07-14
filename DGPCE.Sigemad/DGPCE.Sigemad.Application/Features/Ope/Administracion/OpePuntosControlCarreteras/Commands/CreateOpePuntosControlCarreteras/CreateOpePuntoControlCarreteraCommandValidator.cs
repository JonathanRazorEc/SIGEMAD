using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePuntosControlCarreteras.Commands.CreateOpePuntosControlCarreteras;

public class CreateOpePuntoControlCarreteraCommandValidator : AbstractValidator<CreateOpePuntoControlCarreteraCommand>
{
    public CreateOpePuntoControlCarreteraCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(p => p.Nombre)
            .NotEmpty().WithMessage(localizer["NombreNoVacio"])
            .NotNull().WithMessage(localizer["NombreObligatorio"])
            .MaximumLength(255).WithMessage(localizer["NombreMaxLength"]);
    }
}
