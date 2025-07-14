using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePorcentsOcAE.Commands.CreateOpePorcentajesOcupacionAreasEstacionamiento;

public class CreateOpePorcentOcAECommandVdator : AbstractValidator<CreateOpePorcentOcAECommand>
{
    public CreateOpePorcentOcAECommandVdator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(p => p.IdOpeOcupacion)
           .NotNull().WithMessage(localizer["OpeOcupacionObligatorio"])
           .GreaterThan(0).WithMessage(localizer["OpeOcupacionInvalido"]);

        RuleFor(p => p.PorcentajeInferior)
            .NotNull().WithMessage(localizer["PorcentajeInferiorObligatorio"])
            .GreaterThanOrEqualTo(0).WithMessage(localizer["PorcentajeInferiorInvalido"]);

        RuleFor(p => p.PorcentajeSuperior)
            .NotNull().WithMessage(localizer["PorcentajeSuperiorObligatorio"])
            .GreaterThanOrEqualTo(0).WithMessage(localizer["PorcentajeSuperiorInvalido"]);

        /*
        RuleFor(p => p.FechaValidezHasta)
            .NotEmpty().WithMessage(localizer["FechaValidezHastaObligatorio"]);
        */
    }
}
