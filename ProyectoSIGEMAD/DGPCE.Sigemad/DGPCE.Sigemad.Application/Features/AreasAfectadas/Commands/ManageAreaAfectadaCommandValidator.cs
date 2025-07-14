using DGPCE.Sigemad.Application.Dtos.AreasAfectadas;
using DGPCE.Sigemad.Application.Resources;
using DGPCE.Sigemad.Domain.Constracts;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.AreasAfectadas.Commands;
public class ManageAreaAfectadaCommandValidator : AbstractValidator<ManageAreaAfectadaCommand>
{
    public ManageAreaAfectadaCommandValidator(IStringLocalizer<ValidationMessages> localizer, IGeometryValidator geometryValidator)
    {
        RuleFor(p => p.IdSuceso)
            .GreaterThan(0).WithMessage(localizer["IdSucesoObligatorio"]);

        // Validación para cada elemento de la lista AreasAfectadas
        RuleForEach(command => command.AreasAfectadas)
            .SetValidator(new AreaAfectadaDtoValidator(localizer, geometryValidator))
            .When(d => d.AreasAfectadas.Count > 0);
    }
}

public class AreaAfectadaDtoValidator : AbstractValidator<CreateOrUpdateAreaAfectadaDto>
{
    public AreaAfectadaDtoValidator(IStringLocalizer<ValidationMessages> localizer, IGeometryValidator geometryValidator)
    {
        RuleFor(p => p.FechaHora)
            .NotEmpty().WithMessage(localizer["FechaHoraObligatorio"]);

        RuleFor(p => p.IdProvincia)
           .NotNull().WithMessage(localizer["ProvinciaObligatorio"])
           .GreaterThan(0).WithMessage(localizer["ProvinciaInvalido"]);

        RuleFor(p => p.IdMunicipio)
            .NotNull().WithMessage(localizer["MunicipioObligatorio"])
            .GreaterThan(0).WithMessage(localizer["MunicipioInvalido"]);

        RuleFor(p => p.IdEntidadMenor)
                .GreaterThan(0).WithMessage(localizer["EntidadMenorObligatorio"]);

        RuleFor(p => p.GeoPosicion)
            .Must(geometryValidator.IsGeometryValidAndInEPSG4326).When(p => p.GeoPosicion != null)
            .WithMessage(localizer["GeoPosicionInvalida"]);
    }
}
