using DGPCE.Sigemad.Application.Dtos.DeclaracionesZAGEP;
using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;


namespace DGPCE.Sigemad.Application.Features.DeclaracionesZAGEP.Commands.ManageDeclaracionesZAGEP;
public class ManageDeclaracionesZAGEPCommandValidator : AbstractValidator<ManageDeclaracionesZAGEPCommand>
{
    public ManageDeclaracionesZAGEPCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x => x.IdSuceso)
        .GreaterThan(0).WithMessage(localizer["IdSucesoObligatorio"]);


        RuleForEach(command => command.Detalles)
           .SetValidator(new ManageDeclaracionesZAGEPCommandListValidator(localizer))
            .When(d => d.Detalles != null && d.Detalles.Count > 0);
    }


    public class ManageDeclaracionesZAGEPCommandListValidator : AbstractValidator<ManageDeclaracionZAGEPDto>
    {
        public ManageDeclaracionesZAGEPCommandListValidator(IStringLocalizer<ValidationMessages> localizer)
        {
            RuleFor(x => x.FechaSolicitud)
              .NotNull().NotEmpty().WithMessage(localizer["FechaSolicitudObligatorio"]);

            RuleFor(x => x.Denominacion)
             .NotEmpty().WithMessage(localizer["DenominacionObligatorio"])
             .MaximumLength(510).WithMessage(localizer["DenominacionZAGEPMaxLength"]);

        }
    }
}
