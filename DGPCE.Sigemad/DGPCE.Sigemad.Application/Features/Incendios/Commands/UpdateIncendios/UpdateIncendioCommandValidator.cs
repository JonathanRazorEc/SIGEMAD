using DGPCE.Sigemad.Application.Helpers;
using DGPCE.Sigemad.Application.Resources;
using DGPCE.Sigemad.Domain.Enums;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.Incendios.Commands.UpdateIncendios;

public class UpdateIncendioCommandValidator : AbstractValidator<UpdateIncendioCommand>
{
    public UpdateIncendioCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(p => p.Id)
            .NotEmpty().WithMessage(localizer["IdNoVacio"])
            .NotNull().WithMessage(localizer["IdObligatorio"]);

        RuleFor(p => p.IdTerritorio)
            .IsInEnum().WithMessage(localizer["TerritorioInvalido"])
            .NotEqual(TipoTerritorio.None).WithMessage(localizer["TerritorioObligatorio"]);

        RuleFor(p => p.Denominacion)
            .NotEmpty().WithMessage(localizer["DenominacionNoVacio"])
            .NotNull().WithMessage(localizer["DenominacionObligatorio"])
            .MaximumLength(255).WithMessage(localizer["DenominacionMaxLength"]);

        RuleFor(p => p.FechaInicio)
            .NotEmpty().WithMessage(localizer["FechaInicioObligatorio"])
            .Must(fecha => fecha.Date <= DateTime.UtcNow.Date)
            .WithMessage(localizer["FechaInicioFutura"]);

        RuleFor(p => p.IdClaseSuceso)
            .GreaterThan(0).WithMessage(localizer["ClaseSucesoObligatorio"]);

        RuleFor(p => p.IdEstadoSuceso)
            .GreaterThan(0).WithMessage(localizer["EstadoSucesoObligatorio"]);

        RuleFor(p => p.GeoPosicion)
            .NotNull().WithMessage(localizer["GeoPosicionObligatorio"])
            .Must(GeoJsonValidatorUtil.IsGeometryInWgs84).WithMessage(localizer["GeoPosicionInvalida"]);


        When(p => p.IdTerritorio == TipoTerritorio.Nacional, () =>
        {
            RuleFor(p => p.IdProvincia)
            .NotNull().WithMessage(localizer["ProvinciaObligatorio"])
            .GreaterThan(0).WithMessage(localizer["ProvinciaInvalido"]);

            RuleFor(p => p.IdMunicipio)
            .NotNull().WithMessage(localizer["MunicipioObligatorio"])
                .GreaterThan(0).WithMessage(localizer["MunicipioInvalido"]);
        });

        When(p => p.IdTerritorio == TipoTerritorio.Extranjero, () =>
        {
            RuleFor(p => p.IdPais)
            .NotNull().WithMessage(localizer["PaisObligatorio"])
            .GreaterThan(0).WithMessage(localizer["PaisInvalido"]);

            RuleFor(p => p.Ubicacion)
                .NotEmpty().WithMessage(localizer["UbicacionNoVacio"])
                .NotNull().WithMessage(localizer["UbicacionObligatorio"]);
        });
    }
}
