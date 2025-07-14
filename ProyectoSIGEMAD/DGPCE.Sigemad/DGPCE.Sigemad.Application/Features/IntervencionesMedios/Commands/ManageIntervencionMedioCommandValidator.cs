using DGPCE.Sigemad.Application.Dtos.IntervencionMedios;
using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.IntervencionesMedios.Commands;
public class ManageIntervencionMedioCommandValidator : AbstractValidator<ManageIntervencionMedioCommand>
{
    public ManageIntervencionMedioCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x => x.IdSuceso)
            .GreaterThan(0).WithMessage(localizer["IdSucesoObligatorio"]);

        RuleForEach(x => x.Intervenciones)
         .SetValidator(new CreateIntervencionesValidator(localizer));

    }
}

public class CreateIntervencionesValidator : AbstractValidator<CreateOrUpdateIntervencionMedioDto>
{
    public CreateIntervencionesValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x => x.IdCapacidad)
            .GreaterThan(0).WithMessage(localizer["IdCapacidadObligatorio"]);

        RuleFor(x => x.IdCaracterMedio)
            .GreaterThan(0).WithMessage(localizer["IdCaracterMedioObligatorio"]);

        RuleFor(x => x.NumeroCapacidades)
            .GreaterThan(0).WithMessage(localizer["NumeroCapacidadesObligatorio"]);

        RuleFor(x => x.IdTitularidadMedio)
            .GreaterThan(0).WithMessage(localizer["IdTitularidadMedioObligatorio"]);


        RuleFor(x => x.FechaHoraInicio)
            .NotEmpty().NotNull().WithMessage(localizer["FechaInicioObligatorio"]);

        //RuleFor(p => p.IdProvincia)
        //    .NotNull().WithMessage(localizer["ProvinciaObligatorio"])
        //    .GreaterThan(0).WithMessage(localizer["ProvinciaInvalido"]);

        //RuleFor(p => p.IdMunicipio)
        //    .NotNull().WithMessage(localizer["MunicipioObligatorio"])
        //    .GreaterThan(0).WithMessage(localizer["MunicipioInvalido"]);

    }
}
