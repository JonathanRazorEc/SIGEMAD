using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpePeriodos.Commands.CreateOpePeriodos;

public class CreateOpePeriodoCommandValidator : AbstractValidator<CreateOpePeriodoCommand>
{
    public CreateOpePeriodoCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(p => p.Nombre)
            .NotEmpty().WithMessage(localizer["NombreNoVacio"])
            .NotNull().WithMessage(localizer["NombreObligatorio"])
            .MaximumLength(255).WithMessage(localizer["NombreMaxLength"]);

        RuleFor(p => p.FechaInicioFaseSalida)
            .NotEmpty().WithMessage(localizer["FechaInicioFaseSalidaObligatorio"]);

        RuleFor(p => p.FechaFinFaseSalida)
            .NotEmpty().WithMessage(localizer["FechaFinFaseSalidaObligatorio"]);

        RuleFor(p => p.FechaInicioFaseRetorno)
           .NotEmpty().WithMessage(localizer["FechaInicioFaseRetornoObligatorio"]);

        RuleFor(p => p.FechaFinFaseRetorno)
            .NotEmpty().WithMessage(localizer["FechaFinFaseRetornoObligatorio"]);

    }
}
