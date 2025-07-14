using DGPCE.Sigemad.Application.Helpers;
using DGPCE.Sigemad.Application.Resources;
using DGPCE.Sigemad.Domain.Enums;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpePeriodos.Commands.UpdateOpePeriodos;

public class UpdateOpePeriodoCommandValidator : AbstractValidator<UpdateOpePeriodoCommand>
{
    public UpdateOpePeriodoCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(p => p.Id)
            .NotEmpty().WithMessage(localizer["IdNoVacio"])
            .NotNull().WithMessage(localizer["IdObligatorio"]);


        RuleFor(p => p.Nombre)
            .NotEmpty().WithMessage(localizer["DenominacionNoVacio"])
            .NotNull().WithMessage(localizer["DenominacionObligatorio"])
            .MaximumLength(255).WithMessage(localizer["DenominacionMaxLength"]);

        RuleFor(p => p.FechaInicioFaseSalida)
            .NotEmpty().WithMessage(localizer["FechaInicioFaseSalidaObligatorio"]);

        RuleFor(p => p.FechaFinFaseSalida)
            .NotEmpty().WithMessage(localizer["FechaFinFaseSalidaObligatorio"]);


    }
}
