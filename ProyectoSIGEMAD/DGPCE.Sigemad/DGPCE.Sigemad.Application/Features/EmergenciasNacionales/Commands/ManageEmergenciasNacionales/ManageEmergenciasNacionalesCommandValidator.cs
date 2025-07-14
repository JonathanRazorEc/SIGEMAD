using DGPCE.Sigemad.Application.Dtos.EmergenciasNacionales;
using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;


namespace DGPCE.Sigemad.Application.Features.EmergenciasNacionales.Commands.ManageEmergenciasNacionales;
public class ManageEmergenciasNacionalesCommandValidator : AbstractValidator<ManageEmergenciasNacionalesCommand>
{
    public ManageEmergenciasNacionalesCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x => x.IdSuceso)
        .GreaterThan(0).WithMessage(localizer["IdSucesoObligatorio"]);


        RuleFor(x => x.EmergenciaNacional)
            .SetValidator(new ManageEmergenciasNacionalesCommandObjectValidator(localizer))
            .When(d => d.EmergenciaNacional != null);
    }
}

public class ManageEmergenciasNacionalesCommandObjectValidator : AbstractValidator<ManageEmergenciaNacionalDto>
{
    public ManageEmergenciasNacionalesCommandObjectValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x => x.FechaHoraSolicitud)
          .NotNull().NotEmpty().WithMessage(localizer["FechaHoraSolicitud"]);

        RuleFor(x => x.Autoridad)
         .NotEmpty().WithMessage(localizer["AutoridadSolicitanteObligatorio"])
         .MaximumLength(510).WithMessage(localizer["AutoridadSolicitanteMaxLength"]);

        RuleFor(x => x.DescripcionSolicitud)
         .NotEmpty().WithMessage(localizer["DescripcionSolicitudObligatorio"])
         .MaximumLength(510).WithMessage(localizer["DescripcionSolicitudMaxLength"]);
    }     
}