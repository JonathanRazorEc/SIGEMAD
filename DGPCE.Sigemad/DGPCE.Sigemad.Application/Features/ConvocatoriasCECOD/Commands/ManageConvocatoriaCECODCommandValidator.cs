using DGPCE.Sigemad.Application.Dtos.ConvocatoriasCECOD;
using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;



namespace DGPCE.Sigemad.Application.Features.ConvocatoriasCECOD.Commands;
public class ManageConvocatoriaCECODCommandValidator : AbstractValidator<ManageConvocatoriaCECODCommand>
{
    public ManageConvocatoriaCECODCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x => x.IdSuceso)
        .GreaterThan(0).WithMessage(localizer["IdSucesoObligatorio"]);


        RuleForEach(command => command.Detalles)
           .SetValidator(new ManageConvocatoriaCECODCommandListValidator(localizer))
            .When(d => d.Detalles != null && d.Detalles.Count > 0);
    }

    public class ManageConvocatoriaCECODCommandListValidator : AbstractValidator<ManageConvocatoriaCECODDto>
    {
        public ManageConvocatoriaCECODCommandListValidator(IStringLocalizer<ValidationMessages> localizer)
        {
            RuleFor(x => x.FechaInicio)
              .NotNull().NotEmpty().WithMessage(localizer["FechaInicioObligatorio"]);

            RuleFor(x => x.Lugar)
             .NotEmpty().WithMessage(localizer["LugarObligatorio"])
             .MaximumLength(510).WithMessage(localizer["LugarObligatorioMaxLength"]);

            RuleFor(x => x.Convocados)
             .NotEmpty().WithMessage(localizer["ConvocadosObligatorio"])
             .MaximumLength(510).WithMessage(localizer["ConvocadosMaxLength"]);

        }
    }
}
