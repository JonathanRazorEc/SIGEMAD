using DGPCE.Sigemad.Application.Dtos.NotificacionesEmergencias;
using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;


namespace DGPCE.Sigemad.Application.Features.NotificacionesEmergencias.Commands.ManageNotificacionEmergencia;
public class ManageNotificacionEmergenciaValidator : AbstractValidator<ManageNotificacionEmergenciaCommand>
{
    public ManageNotificacionEmergenciaValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x => x.IdSuceso)
            .GreaterThan(0).WithMessage(localizer["IdSucesoObligatorio"]);

        RuleForEach(x => x.Detalles).SetValidator(new NotificacionEmergenciaDtoValidator(localizer))
                .When(d => d.Detalles != null && d.Detalles.Count > 0);
    }

    public class NotificacionEmergenciaDtoValidator : AbstractValidator<ManageNotificacionEmergenciaDto>
    {
        public NotificacionEmergenciaDtoValidator(IStringLocalizer<ValidationMessages> localizer)
        {

            RuleFor(x => x.IdTipoNotificacion)
             .GreaterThan(0).WithMessage(localizer["IdTipoNotificacionObligatorio"]);

            RuleFor(d => d.FechaHoraNotificacion)
                .NotEmpty().WithMessage(localizer["FechaHoraNotificacionObligatorio"]);

            RuleFor(x => x.OrganosNotificados)
                .NotEmpty().WithMessage(localizer["OrganosNotificadosObligatorio"])
                .MaximumLength(510).WithMessage(localizer["OrganosNotificadosMaxLength"]);

        }
    }
}
