using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePuertos.Commands.CreateOpePuertos;

public class CreateOpePuertoCommandValidator : AbstractValidator<CreateOpePuertoCommand>
{
    public CreateOpePuertoCommandValidator(IStringLocalizer<ValidationMessages> localizer)
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
    }
}
