using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeLineasMaritimas.Commands.CreateOpeLineasMaritimas;

public class CreateOpeLineaMaritimaCommandValidator : AbstractValidator<CreateOpeLineaMaritimaCommand>
{
    public CreateOpeLineaMaritimaCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(p => p.Nombre)
            .NotEmpty().WithMessage(localizer["NombreNoVacio"])
            .NotNull().WithMessage(localizer["NombreObligatorio"])
            .MaximumLength(255).WithMessage(localizer["NombreMaxLength"]);

        RuleFor(p => p.FechaValidezDesde)
            .NotEmpty().WithMessage(localizer["FechaValidezDesdeObligatorio"]);

        /*
        RuleFor(p => p.FechaValidezHasta)
            .NotEmpty().WithMessage(localizer["FechaValidezHastaObligatorio"]);
        */

        RuleFor(p => p.IdOpePuertoDestino)
        .Must((command, idDestino) => idDestino != command.IdOpePuertoOrigen)
        .WithMessage(localizer["OpePuertosOrigenDestinoNoIguales"]);
    }
}
