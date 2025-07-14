using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePuertos.Commands.UpdateOpePuertos;
using DGPCE.Sigemad.Application.Helpers;
using DGPCE.Sigemad.Application.Resources;
using DGPCE.Sigemad.Domain.Enums;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpePuertos.Commands.UpdateOpePuertos;

public class UpdateOpePuertoCommandValidator : AbstractValidator<UpdateOpePuertoCommand>
{
    public UpdateOpePuertoCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(p => p.Id)
            .NotEmpty().WithMessage(localizer["IdNoVacio"])
            .NotNull().WithMessage(localizer["IdObligatorio"]);


        RuleFor(p => p.Nombre)
            .NotEmpty().WithMessage(localizer["DenominacionNoVacio"])
            .NotNull().WithMessage(localizer["DenominacionObligatorio"])
            .MaximumLength(255).WithMessage(localizer["DenominacionMaxLength"]);

        RuleFor(p => p.FechaValidezDesde)
            .NotEmpty().WithMessage(localizer["FechaValidezDesdeObligatorio"]);

        /*
        RuleFor(p => p.FechaValidezHasta)
            .NotEmpty().WithMessage(localizer["FechaValidezHastaObligatorio"]);
        */

    }
}
