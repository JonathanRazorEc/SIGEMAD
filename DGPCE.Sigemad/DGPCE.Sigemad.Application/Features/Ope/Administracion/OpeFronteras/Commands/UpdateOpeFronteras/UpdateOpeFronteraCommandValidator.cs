using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeFronteras.Commands.UpdateOpeFronteras;

public class UpdateOpeFronteraCommandValidator : AbstractValidator<UpdateOpeFronteraCommand>
{
    public UpdateOpeFronteraCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(p => p.Id)
            .NotEmpty().WithMessage(localizer["IdNoVacio"])
            .NotNull().WithMessage(localizer["IdObligatorio"]);


        RuleFor(p => p.Nombre)
            .NotEmpty().WithMessage(localizer["DenominacionNoVacio"])
            .NotNull().WithMessage(localizer["DenominacionObligatorio"])
            .MaximumLength(255).WithMessage(localizer["DenominacionMaxLength"]);


        RuleFor(p => p.TransitoAltoVehiculos)
        .Must((command, transitoAlto) =>
        {
            if (command.TransitoMedioVehiculos == null || transitoAlto == null)
                return true;

            return transitoAlto > command.TransitoMedioVehiculos;
        })
        .WithMessage(localizer["TransitoAltoDebeSerMayorQueMedio"]);

    }
}
