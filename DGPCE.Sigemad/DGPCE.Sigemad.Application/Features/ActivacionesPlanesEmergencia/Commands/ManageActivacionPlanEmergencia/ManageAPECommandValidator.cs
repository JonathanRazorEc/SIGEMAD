using DGPCE.Sigemad.Application.Dtos.ActivacionesPlanes;
using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.ActivacionesPlanesEmergencia.Commands.ManageActivacionPlanEmergencia;

public class ManageActivacionPlanEmergenciaCommandValidator : AbstractValidator<ManageActivacionPlanEmergenciaCommand>
{
    public ManageActivacionPlanEmergenciaCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x => x.IdSuceso)
            .GreaterThan(0).WithMessage(localizer["IdSucesoObligatorio"]);

        RuleForEach(x => x.ActivacionesPlanes).SetValidator(new ActivacionPlanEmergenciaDtoValidator(localizer));
    }
}

public class ActivacionPlanEmergenciaDtoValidator : AbstractValidator<ManageActivacionPlanEmergenciaDto>
{
    public ActivacionPlanEmergenciaDtoValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(d => d.FechaHoraInicio)
            .NotEmpty().WithMessage(localizer["FechaInicioObligatorio"])
            .LessThanOrEqualTo(d => d.FechaHoraFin).When(d => d.FechaHoraFin.HasValue)
            .WithMessage(localizer["FechaInicioDebeSerMenorQueFechaFin"]);

        RuleFor(x => x.Autoridad)
            .NotEmpty().WithMessage(localizer["AutoridadObligatorio"])
            .MaximumLength(100).WithMessage(localizer["AutoridadMaxLength"]);

    }
}
