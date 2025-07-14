using DGPCE.Sigemad.Application.Dtos.CoordinacionCecopis;
using DGPCE.Sigemad.Application.Dtos.CoordinacionesPMA;
using DGPCE.Sigemad.Application.Dtos.Direcciones;
using DGPCE.Sigemad.Application.Resources;
using DGPCE.Sigemad.Domain.Constracts;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Commands;
public class CreateOrUpdateDireccionCommandValidator : AbstractValidator<CreateOrUpdateDireccionCoordinacionCommand>
{
    public CreateOrUpdateDireccionCommandValidator(IStringLocalizer<ValidationMessages> localizer, IGeometryValidator geometryValidator)
    {
        RuleFor(x => x.IdSuceso)
            .GreaterThan(0).WithMessage(localizer["IdSucesoObligatorio"]);

        RuleForEach(x => x.Direcciones)
            .SetValidator(new DireccionDtoValidator(localizer))
            .When(d => d.Direcciones.Count > 0);

        RuleForEach(x => x.CoordinacionesPMA)
            .SetValidator(new CoordinacionPmaDtoValidator(localizer, geometryValidator))
            .When(d => d.CoordinacionesPMA.Count > 0);

        RuleForEach(x => x.CoordinacionesCECOPI)
            .SetValidator(new CoordinacionCecopiDtoValidator(localizer, geometryValidator))
            .When(d => d.CoordinacionesCECOPI.Count > 0);
    }
}


public class DireccionDtoValidator : AbstractValidator<CreateOrUpdateDireccionDto>
{
    public DireccionDtoValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(d => d.IdTipoDireccionEmergencia)
            .GreaterThan(0).WithMessage(localizer["IdTipoDireccionEmergenciaInvalido"]);

        RuleFor(d => d.AutoridadQueDirige)
            .NotEmpty().WithMessage(localizer["AutoridadQueDirigeObligatorio"]);

        RuleFor(d => d.FechaInicio)
            .NotEmpty().WithMessage(localizer["FechaInicioObligatorio"])
            .LessThanOrEqualTo(d => d.FechaFin).When(d => d.FechaFin.HasValue)
            .WithMessage(localizer["FechaInicioDebeSerMenorQueFechaFin"]);

    }
}

public class CoordinacionPmaDtoValidator : AbstractValidator<CreateOrUpdateCoordinacionPmaDto>
{
    public CoordinacionPmaDtoValidator(IStringLocalizer<ValidationMessages> localizer, IGeometryValidator geometryValidator)
    {

        RuleFor(x => x.Lugar)
            .NotEmpty().WithMessage(localizer["LugarObligatorio"]);

        RuleFor(d => d.FechaInicio)
            .NotEmpty().WithMessage(localizer["FechaInicioObligatorio"])
            .LessThanOrEqualTo(d => d.FechaFin).When(d => d.FechaFin.HasValue)
            .WithMessage(localizer["FechaInicioDebeSerMenorQueFechaFin"]);

        RuleFor(p => p.IdProvincia)
           .NotNull().WithMessage(localizer["ProvinciaObligatorio"])
           .GreaterThan(0).WithMessage(localizer["ProvinciaInvalido"]);

        RuleFor(p => p.IdMunicipio)
            .NotNull().WithMessage(localizer["MunicipioObligatorio"])
            .GreaterThan(0).WithMessage(localizer["MunicipioInvalido"]);

        RuleFor(p => p.GeoPosicion)
            .Must(geometry => geometryValidator.IsGeometryValidAndInEPSG4326(geometry))
            .When(p => p.GeoPosicion != null)
            .WithMessage(localizer["GeoPosicionInvalida"]);
    }
}

public class CoordinacionCecopiDtoValidator : AbstractValidator<CreateOrUpdateCoordinacionCecopiDto>
{
    public CoordinacionCecopiDtoValidator(IStringLocalizer<ValidationMessages> localizer, IGeometryValidator geometryValidator)
    {

        RuleFor(x => x.Lugar)
            .NotEmpty().WithMessage(localizer["LugarObligatorio"]);

        RuleFor(d => d.FechaInicio)
            .NotEmpty().WithMessage(localizer["FechaInicioObligatorio"])
            .LessThanOrEqualTo(d => d.FechaFin).When(d => d.FechaFin.HasValue)
            .WithMessage(localizer["FechaInicioDebeSerMenorQueFechaFin"]);

        RuleFor(p => p.IdProvincia)
           .NotNull().WithMessage(localizer["ProvinciaObligatorio"])
           .GreaterThan(0).WithMessage(localizer["ProvinciaInvalido"]);

        RuleFor(p => p.IdMunicipio)
            .NotNull().WithMessage(localizer["MunicipioObligatorio"])
            .GreaterThan(0).WithMessage(localizer["MunicipioInvalido"]);

        RuleFor(p => p.GeoPosicion)
            .Must(geometry => geometryValidator.IsGeometryValidAndInEPSG4326(geometry))
            .When(p => p.GeoPosicion != null)
            .WithMessage(localizer["GeoPosicionInvalida"]);
    }
}